FubuMVC Swagger
===============

This project allows your content negotiation enabled [FubuMVC](https://github.com/DarthFubuMVC/fubumvc) web application
to emit [Swagger specification](http://swagger.wordnik.com/spec)

### How do I get it? ###

We have a [nuget package](https://nuget.org/packages/FubuMVC.Swagger) available.

```PM> Install-Package FubuMVC.Swagger```

#### Building Swagger

To build just run rake grabbing the albacore gem if you don't already have it. 

```rb
gem install albacore
rake
```

### Integration

Add a reference to this project. Why not [use nuget](https://nuget.org/packages/FubuMVC.Swagger)!

Add the following to your FubuRegistry:

```cs
ApplyConvention<SwaggerConvention>();
Services(s=> s.AddService<IActionGrouper, APIRouteGrouper>());
```

This should all be simplified when I find the time to figure out Fubu Bottles. 

### What does this convention do?

Two routes will be added to your application:

```html
api/resources.json
api/{GroupKey}.json
```

The first is for resource discovery which let's Swagger-UI know where all of your resource APIs are defined in JSON. 
The second is for details of each API group present in your app. Currently API routes are assumed to be under /api. 
Hopefully this limitation will go away.

Pointing a Swagger UI web site at ```http://localhost:port/api/``` should render pretty API documentation for all content negotiation enabled actions in your web applicaiton.

### Why do I not see documentation for my actions?

Make sure the actions you wish to document are enabled for "Conneg"

In this example I have two marker interfaces which are used to mark input models on actions which will be APIs. 

```cs
graph.Behaviors
 .Where(x => x.InputType().CanBeCastTo<IApi>() || x.InputType().CanBeCastTo<IUnauthenticatedApi>())
 .Each(x => x.MakeAsymmetricJson());
```

This example will force the result of these actions to be JSON. Note: This configuration is working around a bug in FubuMVC where normal browser usage will return XML for Conneg enabled endpoints. :( 

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