FubuMVC Swagger
===============

This project enables your content negotiation enabled [FubuMVC](https://github.com/DarthFubuMVC/fubumvc) web application to emit [Swagger specification](http://swagger.wordnik.com/spec)

### Build

```rake

I am working on making a Nuget available. 

### Integration

Add a reference to this project. 

Add the following to your FubuRegistry:

```ApplyConvention<SwaggerConvention>();

### What does this convention do?

Two routes will be added to your application:

```
api/resources.json
api/{GroupKey}.json
```

Pointing a Swagger UI web site at ```http://localhost:port/api/``` should render pretty API documentation for all content negotiation enabled actions in your web applicaiton.

### Why do I not see documentation for my actions?

Make sure the actions you wish to document are enabled for "Conneg"

In this example I have two marker interfaces which are used to mark input models on actions which will be APIs. 

```cs
graph.Behaviors
 .Where(x => x.InputType().CanBeCastTo<IApi>() || x.InputType().CanBeCastTo<IUnauthenticatedApi>())
 .Each(x => x.MakeAsymmetricJson());
```

This example will force the result of these actions to be JSON. Note: This configuration is working around a bug in FubuMVC where normal browser usage will return XML for Conneg enabled endpoints. :broken_heart: 

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