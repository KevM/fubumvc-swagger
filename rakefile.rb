require 'albacore'
include REXML
include Rake::DSL

BUILD_NUMBER_BASE = "0.1.0"
PROJECT_NAME = "FubuSwagger"
SLN_PATH = "src/#{PROJECT_NAME}.sln"
SLN_FILES = [SLN_PATH]

NUGET_EXE = File.absolute_path("src/.nuget/nuget.exe")

puts "Loading scripts from build support directory..."
buildsupportfiles = Dir["#{File.dirname(__FILE__)}/buildsupport/*.rb"]
buildsupportfiles.each { |ext| 
	puts "loading #{ext}" 
	load ext 
}

props = {:archive => "build", :testing => "results"}

desc "**Default**, compiles and runs unit tests"
task :default => [:clean,:version,:compile,:test_assemblies,:unit_tests]

desc "Build release version of web site"
task :build_release do 
	Rake::Task["compile"].execute(:target => :RELEASE)
end

desc "build solution"
task :compile => [:version] do |t, args|
	target = args[:target] || :DEBUG
 
	puts "Doing #{target} build" 

	SLN_FILES.each do |f|
		msb = MSBuild.new
		msb.properties :configuration => target
		msb.targets :Clean, :Build
		msb.verbosity = "minimal"
		msb.solution = f
		msb.execute
	end
end

#desc "Copy archives to test folder in order to run unit tests"
output :test_assemblies => [:compile] do |out|
	out.from "#{File.dirname(__FILE__)}"
	out.to "#{props[:testing]}"
	Dir.glob("**/bin/Debug*/*.*"){ |file|
		out.file file, :as => "assemblies/#{File.basename(file)}"
	}	
end

desc "Run unit tests for any dlls that end with 'tests'"
nunit :unit_tests do |nunit|	
	nunit.command = findNunitConsoleExe()
	nunit.assemblies = Dir.glob("results/assemblies/*{T,t}ests.dll").uniq
	nunit.options '/xml=results/unit-test-results.xml'
end

def findNunitConsoleExe
	nunitPackageDirectory = Dir.glob('src/packages/NUnit*').first
	raise "NUnit package was not found under src/packages." if nunitPackageDirectory.nil?
	
	return File.join(nunitPackageDirectory, 'tools/nunit-console.exe')
end

namespace :nuget do

	desc "Run nuget update on all the projects"
	task :update => [:clean] do 
		Dir.glob(File.join("**","packages.config")){ |file|
			puts "Updating packages for #{file}"
			sh "#{NUGET_EXE} update #{file} -RepositoryPath src/packages"
		}
	end

	desc "Build nuget packages"
	task :build => [:build_release] do 
		FileUtils.mkdir_p("results/packages")
		packagesDir = File.absolute_path("results/packages")
		Dir.glob(File.join("**","*.nuspec")){ |file|
			puts "Building nuget package for #{file}"
			projectPath = File.dirname(file)
			Dir.chdir(projectPath) do 
				puts "in project path #{projectPath}"
				sh "#{NUGET_EXE} pack -OutputDirectory #{packagesDir} -Prop Configuration=Release"
			end		
		}
	end

	desc "Deploy packages to nuget gallery."
	task :deploy => [:default,"nuget:build"] do
		packagesDir = File.absolute_path("results/packages")
		Dir.glob(File.join(packagesDir,"*.nupkg")){ |file|
			sh "#{NUGET_EXE} push #{file.gsub(/\//,"\\\\")}"
		}
	end
end 

#desc "Prepares the working directory for a new build"
task :clean do	

	props.each do |key,val|
		FileUtils.rm_r(Dir.glob("#{val}/*"), :force => true) if File.exists?val
		FileUtils.rmdir(val) if File.exists?val  
	end
	# Clean up all bin folders in the source folder
	FileUtils.rm_rf(Dir.glob("**/{obj,bin}"))

end

#desc "Update the version information for the build"
assemblyinfo :version do |asm|
	asm_version = BUILD_NUMBER_BASE + ".0"

	begin
		gittag = `git describe --long`.chomp 	# looks something like v0.1.0-63-g92228f4
		gitnumberpart = /-(\d+)-/.match(gittag)
		gitnumber = gitnumberpart.nil? ? '0' : gitnumberpart[1]
		commit = `git log -1 --pretty=format:%H`
	rescue
		commit = "git unavailable"
		gitnumber = "0"
	end
	build_number = "#{BUILD_NUMBER_BASE}.#{gitnumber}"
	puts "Git based version is #{build_number}"
	asm.trademark = commit
	asm.version = build_number
	asm.file_version = build_number
	asm.custom_attributes :AssemblyInformationalVersion => build_number
	asm.output_file = 'src/CommonAssemblyInfo.cs'
end