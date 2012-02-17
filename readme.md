FubuMVC Swagger
===============

This project helps your [FubuMVC](https://github.com/DarthFubuMVC/fubumvc) web application
 generate API documentation via [Swagger](http://swagger.wordnik.com/).
 
[![swagger demo](https://github.com/KevM/fubumvc-swagger/raw/gh-pages/images/hellofubuswagger.png)](http://fubuswagger.apphb.com/)

See our [Hello Swagger](http://fubuswagger.apphb.com/) live demo.

### How do I get it? ###

We have a [nuget package](https://nuget.org/packages/FubuMVC.Swagger) available.

```PM> Install-Package FubuMVC.Swagger```

#### Building Swagger

To build just run rake grabbing the albacore gem if you don't already have it. 

```rb
gem install albacore
rake
```

### How do I put this in my peanut butter?

**Note:** This should all be simplified when I find the time to figure out Fubu Bottles. 

Your API documented should support content negitiation and be grouped under the route **/api**. Take a look at the [HelloSwagger](https://github.com/KevM/fubumvc-swagger/tree/master/src/HelloSwagger) for examples. 

#### Checklist

Add a reference to this project. Why not [use nuget](https://nuget.org/packages/FubuMVC.Swagger)!

Add the following to your FubuRegistry:

```cs
ApplyConvention<SwaggerConvention>();
Services(s=> s.AddService<IActionGrouper, APIRouteGrouper>());
```

Copy the swagger-ui directory into your **/content** direcotry.

Launch your web app and take a look at the **/api** to see if it is working.

### What does this convention do?

Three routes will be added to your application:

```html
GET /api
```
This route serves up the Swagger-UI page currently embedded into FubuSwagger. _This part needs work._

```html
GET /api/resources.json
```
Swagger UI does some resource discovery and uses the output of this route to find all of the API groups in your project. 

```html
GET /api/{GroupKey}.json
```
Details of each API group present in your app. 

Pointing a Swagger UI web site at ```http://localhost:port/api/``` should render pretty API documentation for your web application.

### Why do I not see documentation for my actions?

Make sure the actions you wish to document are enabled for "Conneg"

In this example I have two marker interfaces which are used to mark input models on actions which will be APIs. 

```cs
graph.Behaviors
 .Where(x => x.InputType().CanBeCastTo<IApi>() || x.InputType().CanBeCastTo<IUnauthenticatedApi>())
 .Each(x => x.MakeAsymmetricJson());
```

This example will force the result of these actions to be JSON. Note: This configuration is working around a bug in FubuMVC where normal browser usage will return XML for Conneg enabled endpoints. :( 

Better yet take a look at the [HelloSwagger](https://github.com/KevM/fubumvc-swagger/tree/master/src/HelloSwagger) demo application and see how it is wired up and organized.

### How do I add more detail to my APIs

You can use data annotations to mark up your input models and their properties.

```cs
[Description("Workflow object history")]
public class HistoryRequest : IApi 
{
    [Required, Description("Type of workflow object. Typically this is 'case'.")]
    [AllowableValues("case", "subcase", "solution", "<any workflow object name>")]
    public string Type { get; set; }
    [Required, Description("Id of the workflow object.")]
    public string Id { get; set; }

    [Description("Limit the amout of history returned the given number of days. When this parameter is not specified. All history items will be returned.")]
	public int DaysOfHistory { get; set; }
}
```