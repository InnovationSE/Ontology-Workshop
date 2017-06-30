# Coding Session: Instantiating the Model

In this session, we will go through the instantiation of an ontology model (classes and relationships) programmatically. 
We will rely on a set of annotated classes and an Objet-Mapping-Library for RDF to generate a topology instance specific to a site.
The generated topology can be in memory, or stored in an RDF persistent store. It can also be queried in memory or from a peristent store.
![](/coding-session/Figures/yourLibrary.png)

This library can be part of an IoT application, embedded in a commissioning tool or deployed on a system.

### Disclaimer:

In this guide, we rely on _RomanticWeb_ (**R**elational **O**bject **M**apper for the sem**ANTIC WEB**, an open source library for .Net. This API has been identified recently therefore it might have some unidentified bugs or issues. The aim of this session is to give hands on practice to start generating ontologies from code. We try to provide a starting point, a first step to help a new designer to develop ontologies.


## Table of Contents

- [Prerequisite](#prerequisite)
- [Setup](#setup)
1. [Basic Concepts](#1-basic-concepts)
2. [Prepare the Context Factory](#2-prepare-the-context-factory)
3. [Instantiate](#3-instantiate)
4. [Load and LINQ Queries](#4-load-and-linq-queries)
5. [Physical Location](#5-physical-location)
6. [Units and Quantities](#6-units-and-quantities)
7. [Create an RDF database](#7-create-an-rdf-database)
8. [Insert Your Topology](#8-insert-your-topology)
9. [SPARQL Queries and Reasoning](#9-sparql-queries-and-reasoning)
10. [Configuring a Persistent Store](#10-configuring-a-persistent-store)
11. [Next Steps](#11-next-steps)


  

# Prerequisite
1. _Visual Studio 2017_ installed [download](https://www.visualstudio.com/downloads/), you can select community version download.
2. A _Java Virtual Machine_ (version > 1.8) [download JVM 1.8](http://www.oracle.com/technetwork/java/javase/downloads/jre8-downloads-2133155.html)
3. _Protégé_ is an open source ontology editor provided by Stanford University. [download Protégé](http://protege.stanford.edu/products.php#desktop-protege). 
4. An Ontology/RDF Database, we choose _Stardog_, other stores exist as well. 
   [download Stardog Community](http://www.stardog.com/)
5. Once you receive an email from _Stardog_ it will contain:
   1. Zip file of the binaries, unzip it and place it in a folder of your choice **$Stardog**.
   2. Binary License, place this file in **$Stardog** folder contained in your Stardog installation location
   3. Copy [_RunSD.bat_](/coding-session/Prerequisite) in the **$Stardog\bin** folder
   4. Run the bat file


# Setup
1. Create a new C# project as **Console Application .Net Framework**
2. Install the following `Nuget` dependencies by searching _RomanticWeb_, we install the prerelease **version 1.0.0-rc1**. 
_RomanticWeb_ is a Relation Object Mapping for semANTIC WEB in .Net. Other Object Mapping for RDF exist and can be used such as [Trinity](https://bitbucket.org/semiodesk/trinity)
	1. RomanticWeb
	2. RomanticWeb.dotNetRDF
	3. RomanticWeb.JsonLd
	4. RomanticWeb.Contracts
3. Install the dotNetRDF library, version 10.13-pre7
4. Install the VDS.Common library, version 1.8.0-pre1
5. Install all the updates suggested by the `Nuget` manager

This session relies on the _RomanticWeb_ library, you can always refer to it for more details [documentation](http://romanticweb.net/docs/basic-usage/mapping/)

# 1 Basic Concepts
In this step, we will a set of devices topology classes and instantiate it. We define the following 5 classes:
1. Node an abstract mother class of the previous 4 classes
2. ES the EnterpriseServer
3. AS the AutomationServer, is monitored by the EnterpriseServer. The AS controls the Controller
4. Controller, hasPoint a point.
5. Point of measure.

First, let's create the annotations/properties on our 5 classes/interfaces. The annotations are handled later by the RomanticWeb library to create the Ontology Classes and Instances through the Object-Relational-Mapping library.
As shown below, the main concept is the `IEntity` which represents a single RDF resource. All entities must inherit from the IEntity interface.

```C#
    public interface INode : IEntity
    {
        [Property("http://www.schneider-electric.com/Buildings#hasUID")]
        string UID { get; set; }

        [Property("http://www.schneider-electric.com/Buildings#hasName")]
        string Name { get; set; }
    }
```
The INode concept has a unique identifier (UID) and a user friendly name (Name). As shown in the previous code sinppet, UID has a `[Property("http://www.schneider-electric.com/Buildings#hasUID")]` which maps those attributes to their correspondant RDF predicates. Similarly, `Name` has also its own property.

`ES`, `AS`, `Controller`, and `Point` all inherit from `INode`. Therefore, their interface will extend the `INode` interface as shown below:
```C#
    [Class("http://www.schneider-electric.com/Buildings#EnterpriseServer")]
    public interface IES : INode
    {
        [Collection("http://www.schneider-electric.com/Buildings#monitors")]
        ICollection<IAS> Monitors { get; set; }
    }
```
`IES` has the following annotation ```[Class("http://www.schneider-electric.com/Buildings#EnterpriseServer")]``` which references the RDF Class type of the EnterpriseServer. The Class annotation is absent in the `INode` code snipet which informs _RomanticWeb_ that the `INode` Class will not be generated.

## {#..#} Complete the code
-----------------------------
Create three interfaces IAS, IController, and IPoint with the following,
- [ ] IAS with the following Class property `[Class("http://www.schneider-electric.com/Buildings#AutomationServer")]`
- [ ] IController with the following Class property `[Class("http://www.schneider-electric.com/Buildings#Controller")]`
- [ ] IPoint with the following Class property `[Class("http://www.schneider-electric.com/Buildings#Point")]`
- [ ] IAS controls IController, `controls` has the following property `[Collection("http://www.schneider-electric.com/Buildings#controls")]`
- [ ] IController hasPoint IPoint `hasPoint` has the following property ` [Collection("http://www.schneider-electric.com/Buildings#hasPoint")]`
- [ ] IPoint 
	* hasType (string) (`[Property("http://www.schneider-electric.com/Buildings#hasType")]`)
	* hasUnit (string) (` [Collection("http://www.schneider-electric.com/Common#hasUnit")]`)


The source code of this step can be found [here](/coding-session/Code/BasicConcepts).
Make sure you update the namespace `BMSOnt` with the one you used in your solution

# 2 Prepare the Context Factory
Now that the interfaces are annotated, the classes can be instantiated through an entity context factory, but first we need to prepare the context factory with the mappings. The entity context is similar to `ObjectContext` or `DbContext` from [EntityFramework](https://github.com/aspnet/EntityFramework6) or the `ISession` from [NHibernate](http://nhibernate.info/). _RomanticWeb_ allows the _Fluent_ mode to instiate classes as well as shown [here](http://romanticweb.net/docs/basic-usage/mapping/).

First, lets declare the three variables:
```C#
static IEntityContext context;
static ITripleStore store;
static string clientURI = "http://www.schneider-electric.com/Buildings/Lund";
```

Then we can prepare the contextFactory by linking the mappings from the C# interfaces and their annotations to the RDF concepts.
```C#
public static void InitContext()
 {
  var contextFactory = new EntityContextFactory();
  contextFactory.WithMappings((MappingBuilder builder) =>
  {
   builder.FromAssemblyOf<INode>();
  });

  store = new TripleStore();
  contextFactory.WithDotNetRDF(store);
  contextFactory.WithMetaGraphUri(new Uri(clientURI));
  context = contextFactory.CreateContext();
 }
```
The context factory can be linked to an actual RDF store. In the previous code snippet, an in-memory triple store [dotNetRDF](http://www.dotnetrdf.org/) backend is used.
In section [Configuring a persistent store](#10-configuring-a-persistent-store), we go through configuring an actual persistent store, Stardog.

You will rely on the following imports:
```C#
using RomanticWeb;
using RomanticWeb.DotNetRDF;
using RomanticWeb.DotNetRDF.Configuration;
using RomanticWeb.Entities;
using RomanticWeb.Mapping;
using RomanticWeb.Ontologies;
using VDS.RDF.Writing;
using VDS.RDF;
```

# 3 Instantiate
Now that the interfaces, annotations, and mappings in the context factory are all initialized, we can start instantiating the classes.

In the following, we create an EnterpriseServer which monitors two AutomationServers.
```C#
   IAS as1 = context.Create<IAS>(new Uri(clientURI + "#as1"));
   as1.Name = "AutomationServer1";
   as1.UID = "AutomationServer1-123";
   
   IAS as2 = context.Create<IAS>(new Uri(clientURI + "#as2"));
   as2.Name = "AutomationServer2";
   as2.UID = "AutomationServer2-123";
  
   IES es = context.Create<IES>(new Uri(clientURI + "#es1"));
   es.Name = "EnterpriseServer1";
   es.UID = "abc-123";
   es.Monitors.Add(as1);
   es.Monitors.Add(as2);
```

## {#..#} Complete the code
-----------------------------
Create and connect the following (Do not give the elements the same URIs, Names, or UIDs) ![](/coding-session/Figures/Topology.JPG)
- [ ] 8x IPoints, you can use Unit(DegreeC | Wh) and Type (Temperature | Energy)
- [ ] 4x IControllers, [c1 .. c4]
- [ ] 2x IAS, [as1, as2]
- [ ] 1x IES, es1
Then
- [ ] Each IController controls 2x IPoints
- [ ] Each IAS controls 2x IController
- [ ] IES monitors 2x IAS

## Commit Your Topology
After you have created your topology, it is necessary to commit it to the context factory through: 
```C# 
context.Commit();
```
The `Commit` function will store the topology created through the context in the used store (in memory or a persistent store).

## Save Your Topology to File
Once, you are satisfied with your topology, you can save it to a file on your disk.
```C#
    IGraph g = new Graph();
    IList graphList = store.Graphs.ToList();
    foreach(IGraph currentGraph in graphList)
    {
 	g.Merge(currentGraph);
    }
            
    RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
    rdfxmlwriter.Save(g, "C:\\output.rdf");
```

In future steps, we will go through how to save it in a persistent store.

The source code of this step can be found [here](/coding-session/Code/Instantiate)

# 4 Load and LINQ Queries

The context factory create the instances and allow us to retrieve them based on the Uri. For example, the following code loads an `es` instance created earlier.
```C#
IES es = context.Load<IES>(new Uri(clientURI + YOUR_ES_URI));
```


In addition to loading the instances, it is also possible to use the Language Integrated Query. LINQ has become a de-facto standard in .Net to allow programmers to query data in an expressive manner. _RomanticWeb_ transforms a subset of the LINQ queries into [SPARQL](https://www.w3.org/TR/rdf-sparql-query/).

_RomanticWeb_ supports the following features of LINQ:
* Select
* SelectMany
* Subqueries
* Where
* Skip/Take
* Ordering
* First/FirstOrDefault & Single/SingleOrDefault
* Any/All
* Count

There are two forms to query with LINQ, typed and untyped:
```C#
IQueryable<IEntity> untypedQuery = context.AsQueryable();
IQueryable<IPoint> typedQuery = context.AsQueryable<IPoint>();
```

The following query example retrieves all the points in the context:
```C#
IQueryable<IEntity> untypedQuery = from p in context.AsQueryable()
                                   where p is IPoint
                                   select p;
                                   
foreach (IPoint point in untypedQuery)
 {
   Console.WriteLine("- " + point.Name);
 }
```

## {#..#} Complete the code
-----------------------------
- [ ] Print your topology by first loading your ES and then going through the whole topology, as shown in the figure below ![](/coding-session/Figures/Topology.JPG)
- [ ] Query Controllers with Controller.Name == "Controller1"
- [ ] Query all the Controllers of an EnterpriseServer with Point type == "Temperature", start with the EnterpriseServer.

The source code of this step can be found [here](/coding-session/Code/LINQ)

# 5 Physical Location
The logical topology (controls, monitors, senses, etc) used previously is just one view which can be found in a system. Other views also exist such as the physical location.

We add in this step the physical location view to our topology. First, let's define the concept of Location (`ILocation`) as follows:
```C#
 public interface ILocation : IEntity
    {
       [Property("http://www.schneider-electric.com/Buildings#hasUID")]
        string UID { get; set; }

       [Property("http://www.schneider-electric.com/Buildings#hasName")]
        string Name { get; set; }
    }
```
The `ILocation` interface defines a UID and a Name. Now, let's us define the concept of a Building (`IBuilding`) as follows:
```C#
 [Class("http://www.schneider-electric.com/Buildings#Building")]
    public interface IBuilding : ILocation
    {
        [Collection("http://www.schneider-electric.com/Buildings#hasLocationFloor")]
        ICollection<IFloor> HasFloor { get; set; }
    }
```
An `IBuilding` extends `ILocation` and has a collection of `IFloor` related by the Uri `hasPhysicalLocation`

## {#..#} Complete the code
-----------------------------
- [ ] Create IFloor which extends `ILocation` and has a collection of `IRoom` related by the Uri `hasLocationRoom`
- [ ] Create IRoom whcih extends `ILocation`
- [ ] Update `INode` which can now have a location `HasLocation` related by the Uri `hasLocation`
- [ ] Create a LocationTopology with 8x Rooms [R1 .. R8], 2x Floors [F1, F2] and 1 Building [B1]. The rooms [R1..R4] in Floor F1, the rooms [R5 .. R8] in F2, and F1, F2 in B1.
- [ ] Add a physical location to your points, controllers, automation servers, and enterprise server as follows:

Point | Room        
---- | --------     
Point1 | Room1	    
Point2 | Room2      
Point3 | Room3
Point4 | Room4
Point5 | Room5
Point6 | Room6
Point7 | Room7
Point8 | Room8

The Controllers:

Controller1 | Room        
---- | -------- 
Controller1 | Room1
Controller2 | Room4
Controller3 | Room2
Controller4 | Room5

The AutomationServers:

AutomationServer | Floor
---- | -------- 
AS1 | F1
AS2 | F2

The EnterpriseServer:

EnterpriseServer | Building
---- | -------- 
ES1 | B1

The source code of this step can be found [here](/coding-session/Code/PhysicalLocation)

# 6 Units and Quantities

Previously, the units and measures were set as strings, however, it is preferrable when it is possible to reuse existing elements in the ontology development. Typically, the units and measures can be reused and are part of the common concepts that we can reuse inside our organisation.

One possible solution is to enumerate the Quantities as follows with their description:
```C#
public enum Quantity
    {
        [Description("http://www.schneider-electric.com/Buildings#Temperature")]
        Temperature,

        [Description("http://www.schneider-electric.com/Buildings#Energy")]
        Energy,
    }
```

And the Units as follow:

```C#
public enum Unit
    {
        [Description("http://www.schneider-electric.com/Buildings#DegreeC")]
        Celsius,

        [Description("http://www.schneider-electric.com/Buildings#Wh")]
        Watt_Hour,
    }
```

For simplicity, we attached the quantities to the `Buildings` ontology. However, Units and Quantities are common accross our organisation, therefore, in reality they have the common ontology uri `http://www.schneider-electric.com/Common/Units`

When we generate our topology, we want to refer to these already existing concepts. Therefore, we need to change in `IPoint` the `string` to `Uri` as follows:

```C#
 [Class("http://www.schneider-electric.com/Buildings#Point")]
    public interface IPoint : INode
    { 
        [Property("http://www.schneider-electric.com/Buildings#hasType")]
        Uri Type { get; set; }

        [Collection("http://www.schneider-electric.com/Buildings#hasUnit")]
        Uri Unit { get; set; }
    }
```

We prepared a helper Class to read the description of the enum, copy it to your code [Utility.cs](/coding-session/Code/UnitsAndMeasures/Utility/). You can use it as follows in your code:

```C#
point2.Unit = new Uri(Utility.GetDescriptionFromEnumValue(Unit.Celsius));
```

## {#..#} Complete the code
-----------------------------
- [ ] Update your code with the two enumrations `Units` and `Quantity`
- [ ] Update `IPoint` by changing the `string` into `Uri`for `Type` and `Unit`
- [ ] Update your topology

The source code of this step can be found [here](/coding-session/Code/UnitsAndMeasures)

In the previous steps, you implemented several interfaces and assigned their properties manually such as the `IPoint`, `IController`, `IAS`, and many others. In order to avoid IoT developpers writing all of this code, we are working on a [*Model Driven Engineering*](https://en.wikipedia.org/wiki/Model-driven_engineering) approach to generate the code directly from the ontology previously designed as in the [Protégé session](/protege-session/README.md). More details are presented in the [Coming soon](#library-generator) section.


# 7 Create an RDF database
Make sure _Stardog_ is already running, if not run the following bat file [runSD.bat](/coding-session/Prerequisite).

Connect to your local instance of _Stardog_ through [http://localhost:5820](http://localhost:5820).
The default credentials are:
```
username: admin
password: admin
```

Once you are logged in, click on `Databases`
![](/coding-session/Figures/createone.png)

Then, you follow these three steps:
1. Name your Database
2. Add the namespaces that you used in your code. This will allow us to use namespaces `Lund` and `bldgs` in our queries instead of the whole Uris `http://www.schneider-electric.com/Buildings/Lund#`.
   1. `lund=http://www.schneider-electric.com/Buildings/Lund#`
   2. `bldgs=http://www.schneider-electric.com/Buildings#`
3. Click on finish to deploy your database

![](/coding-session/Figures/Namespace.png) 

<!--
## Enable Graph Queries
Now that we deployed your database, you need to enable Graph queries by following these five steps:

1. Turn off your database
2. Edit
![Turn it off and Edit](/coding-session/Figures/editdb.png)

3. Scroll down and look for the Query section
![Enable Query](/coding-session/Figures/queryAll.png)
4. Save your changes
![Save Changes](/coding-session/Figures/savedb.png)
5. Turn it back on
![Turn it back on](/coding-session/Figures/turniton.png)
-->

# 8 Insert Your Topology
Now that the store is ready, you will insert your topology manually.

First, click on `Browse`
![](/coding-session/Figures/browse.png)

Then click on `Add` and choose your generated file `C:\\output.rdf` from [step 3](#3-instantiate)
![](/coding-session/Figures/add.png)

If you didn't complete the previous steps, you will not have all the elements for the following steps. 
Therefore, we recommend that you use the following [output.rdf](/coding-session/Ontology/output.rdf) instead of your version.

For now, you inserted your topology manually, in future steps you will configure the `Context Factory` to automatically insert it upon the context commit.

Finally, **Refresh your browser** then click on `Browse` again to navigate accross your topology.
As shown below, you can notice there are two views, the _Classes_ and the _Property_ view. Click on _EnterpriseServer_ and navigate your topology.
![](/coding-session/Figures/navigate.png)

# 9 SPARQL Queries and Reasoning

Previously, we used LINQ to query from your code. While LINQ is so powerfull and allow us to retrieve our entities. Another standard query language [SPARQL](https://www.w3.org/TR/sparql11-query/) can be used to retrieve information from any ontology store. 

```
You might be wondering why can't we just use SQL? 
You can but you will not leverage the full power of the ontologies and the reasoner.
SQL was designed to query tablular data.
The ontology structure is more evolved than the tabular one.
Therefore, the need to propose a new query language more fitted to the new structure has emerged. 
SPARQL was standardized at the World Wide Web Consortium (W3C)
```

## SPARQL With No Reasoning 

SPARQL is designed to query an ontology structure, therefore, it is triplets based.
In this step, we will write the queries in the WebUI tab `Query` as shown below
![](/coding-session/Figures/query.png)


You can also execute queries from your code: 
```C#
 StardogConnector connector = new StardogConnector("http://localhost:5820", storeId, "admin", "admin");
 SparqlResultSet results = (SparqlResultSet)connector.Query("Select ?a ?b ?c Where {?a ?b ?c.}");
 if (results is SparqlResultSet)
 {
	//Print out the Results
    SparqlResultSet rset = (SparqlResultSet)results;
    foreach (SparqlResult result in rset)
		{
			Console.WriteLine(result.
				ToString().
                Replace("http://www.w3.org/1999/02/22-rdf-syntax-ns#", "type:").
                Replace("http://www.schneider-electric.com/Buildings/Lund#", "lund:").
                Replace("http://www.schneider-electric.com/Buildings#", "bldgs:"));
		}
        Console.ReadLine();
 }
```
![](/coding-session/Figures/bug.png) **While preparing for this workshop, we identified a bug in the  StardogConnector. Therefore, it cannot be used for Reasoning in part [9b](#sparql-with-reasoning).**

We proposed an extension to support reasoning and already submitted a [pull request](https://github.com/dotnetrdf/dotnetrdf/pull/109) to dotNetRDF. 

**You may also use a REST Client such as [Postman](https://chrome.google.com/webstore/category/extensions?hl=en) for example.**


**It is preferrable to use the WebUI of Stardog for this step.**

Now let's start with some basic SPARQL queries.
### i. Generic Query
---------------
```SQL
Select ?a ?b ?c
Where
{
 ?a ?b ?c
}
```
This query matches all the triplets in the database, the `?a`, `?b`, and `?c` denotes three variables that the query needs to match.

### ii. Relation Based
---------------------
Let's replace `?b` with a relation you used in your code `http://www.schneider-electric.com/Buildings#monitors`.
```SQL
Select ?a ?c
Where
{
 ?a <http://www.schneider-electric.com/Buildings#monitors> ?c
}
```

You may export the results in several format, csv, tsv and others. Any standard SPARQL endpoint returns one of the standard supported formats [Json](https://www.w3.org/TR/sparql11-results-json/), [Xml](https://www.w3.org/TR/rdf-sparql-XMLres/), [CSV/TSV](https://www.w3.org/TR/sparql11-results-csv-tsv/), and others. For example, since CSV is  compatible with PowerBI, we were able to connect PowerBI to the ontology store. The Proof of Concept is described and published in two IEEE conferences, we added the [IEEE papers](/coding-session/Papers/)

### iii. Relation Based with a namespace
------------------------------------
We can replace this uri with a shorter form, since we already added the namespaces in the database upon its creation.
The uri `http://www.schneider-electric.com/Buildings#monitors` can be replaced by `bldgs:monitor`.

```SQL
Select ?a ?c
Where
{
 ?a bldgs:monitors ?c
}
```

### iv. Querying for a specific element
------------------------------------
The EnterpriseServer monitors two AutomationServers. We will try to query only one of them in the following. We replace `?c` with the AutomationServer1, in other words, the query requests: a Node monitoring the AutomationServer1.

```SQL
Select ?a
Where
{
 ?a bldgs:monitors lund:as1
}
```

```
We replaced ?c in the query with `lund:as1` since it was used to create as1 during the instantiation, as follows:
 IAS as1  = context.Create<IAS>(new Uri(clientURI + "#as1"));
 as1.Name = "AutomationServer1";
 as1.UID  = "AutomationServer1-123";
If you used another uri, you need to replace it with it.
```



### v. Querying based on the AS Name string property
------------------------------------
The Uri can be used in the queries, however this assume that remember the uri you used to create the instance. Instead of remembering the uri, we can query based on different parameters. In the `IES` interface, the `hasName` property was declared as a string. Therefore, in SPARQL, we need to specify the type to be filtered upon.

```SQL
Select ?a
Where
{
 ?a bldgs:monitors ?c.
 ?c bldgs:hasName "AutomationServer1"^^xsd:string.
}
```
Notice that a point `.` is added at the end of each line. It indicates to SPARQL that the condition continues. There is a short form where the left part of the triples can be omitted by using the following character in the queries `;`.

### vi. Include The Physical Location
------------------------------------
Previously, the queries targetted only one dimension, the logical connectivity. However, we can combine several dimensions. In this query, we will include the physical locations by using the relation `bldgs:hasPhysicalLocation`.

```SQL
Select ?a ?locationA ?locationC
Where
{
 ?a bldgs:monitors ?c.
 ?c bldgs:hasName "AutomationServer1"^^xsd:string.
 ?a bldgs:hasPhysicalLocation ?locationA.
 ?c bldgs:hasPhysicalLocation ?locationC.  
}
```

### vii. Retrieve The Types
------------------------------------
In the previous queries, we are still dealing with uri, in this query we will retrieve the types of these uri for a better understanding. The type is retrieved through the *`a`* relation.

```SQL
Select ?a ?locationA ?locationC ?typeA ?typeC ?typeLocationA ?typeLocationC
Where
{
 ?a bldgs:monitors ?c.
 ?c bldgs:hasName "AutomationServer1"^^xsd:string.
 ?a bldgs:hasPhysicalLocation ?locationA.
 ?c bldgs:hasPhysicalLocation ?locationC.
 ?a a ?typeA.
 ?c a ?typeC.
 ?locationA a ?typeLocationA.
 ?locationC a ?typeLocationC.  
}
```

### viii. Query The Units and Quantities
------------------------------------
Previously, we assigned a physical location to the points and controllers. The follow query retrieves all the points and their controllers along with their units and physical locations.
```sql
Select ?cont ?p ?whatIsTheUnit ?contLocation ?pointLocation
Where
{
 ?cont bldgs:hasPoint ?p.
 #?p bldgs:hasType bldgs:Temperature.
 ?p bldgs:hasUnit ?whatIsTheUnit.
 ?cont bldgs:hasPhysicalLocation ?contLocation.
 ?p bldgs:hasPhysicalLocation ?pointLocation. 
}
```
The _#_ is a special character for comments. You can uncomment and rexecute the query.

In all of these previous queries the reasoner was not invoked. The following step will rely on the reasoner.

## SPARQL With Reasoning
According to [[1]](https://en.wikipedia.org/wiki/Inference_engine), "_an inference engine is a component of a system that applied logical rules to the knowledge base to deduce new information._
A semantic reasoner is a broader concept then an inference engine, it relies on wider and richer concepts to extract new information [[2]](https://en.wikipedia.org/wiki/Semantic_reasoner)."

In _Stardog_ the reasoner is already embedded, and it can be invoked through a query form the Web UI. 
```
In case you are wondering,
it is also possible to embedd a reasoner (in memory) outside of an ontology store.
```

In order to leverage the reasoner, first we must insert the model in the following step.

### Insert Your Model
In the previous session, you designed an ontology with _Protégé_ where several properties where added as shown below.

![](/coding-session/Figures/ontologyProperties.png)

For example, `connectsTo` is `Transitive` and is an `inverse Of` `isConnectedTo`.

In our code, we made sure to reuse the same Uri when we were declaring the classes and their relationships, therefore, if we imported the ontology (model) to the database, we can now leverage all of these advanced properties that you defined previously.

To import the model, download the ontology [Buildings.owl](/coding-session/Ontology/) and add it to the current database by following the same steps in [8](#8-insert-your-topology).

Once you inserted your model and refreshed your browser, your topology will have the following rendering
![](/coding-session/Figures/withYourModel.png)



![](/coding-session/Figures/bug.png) **A WebUI Bug in _Stardog 5.0-RC_**

A bug has been identified and reported on the latest release candidate version of the WebUI _Stardog 5.0-RC_ in the WebUI when activating the reasoner. 
Therefore, the following step works only on _Stardog versions < 5_. 
You can still run the following steps from the command line.

1. Open a Command Prompt (cmd.exe) 
2. Go the **$Stardog\bin** (cd $Stardog\bin) 
![](/coding-session/Figures/cmd.png)
3. Use the following command which contains the database name and the query:
```sh
stardog query myDbTest --reasoning "select ?a {?a a bldgs:Node.}"
```

**You may also use a Rest client to query _Stardog_**
```
http://localhost:5820/myDbTest/query?reasoning=true&query=select ?a {?a a bldgs:Node.}
```

The above query returns all the nodes instances in your topology. The reasoner was able to retrieve such information based on the `SubClassOf` properties we defined earlier.

Try the same query without the reasoning enabled.

Now add the following filter to your query:
```sql
Select ?a ?type 
{?a a bldgs:Node.  
?a rdf:type ?type.}
```

This query requests the `type` of the parameter `?a` we are querying.



### i. Query With Transitivity
------------------------------
In your code, the EnterpriseServer (respectively AutomationServer) `monitors` (respectively `controls`) the AutomationServer (respectively the Controller).

The figure above shows `connectsTo` as is a mother relation of `controls` and `monitors` and it is also transitive.
The following query will show all the AutomationServers and Controllers connected to the EnterpriseServer.
```sql
select ?a ?b
{
 ?a bldgs:connectsTo ?b.
 ?a a bldgs:EnterpriseServer.
}
```

Copy the following in your `cmd.exe` window:
```
stardog query myDbTest --reasoning "select ?a ?b {?a bldgs:connectsTo ?b. ?a a bldgs:EnterpriseServer.}"
```


### ii. Query With Inverse
---------------------------
As shown in the figure above, `connectsTo` has `isConnectedTo` as an inverse relation.
```sql
select ?a ?b
{
 ?b bldgs:isConnectedTo ?a.
 ?a a bldgs:EnterpriseServer.
}
```
We still have the same results since ?b and ?a are inversed in the query and `isConnectedTo` is declared transitive as well.

Copy the following in your `cmd.exe` window:
```
stardog query myDbTest --reasoning "select ?a ?b { ?b bldgs:isConnectedTo ?a. ?a a  bldgs:EnterpriseServer. }"
```

### iii. Query The Logical Topology
---------------------------------
We almost got the whole logical topology but we are still missing the points. 
The following query selects the points.
```sql
select ?a ?b ?point
{
 ?a bldgs:connectsTo ?b.
 ?a a bldgs:EnterpriseServer.
 OPTIONAL
 {
  ?b bldgs:hasPoint ?point.
 }
}
```

Copy the following in your `cmd.exe` window:
```
stardog query myDbTest --reasoning "select ?a ?b ?point {  ?a bldgs:connectsTo ?b.  ?a a bldgs:EnterpriseServer.  OPTIONAL  {  ?b bldgs:hasPoint ?point. } }"
```

### iv. Add The Physical Location
---------------------------------
```sql
select ?a ?b ?point ?EsLocation ?bLocation ?pointLocation 
{
 ?a bldgs:connectsTo ?b.
 ?a a bldgs:EnterpriseServer.
 ?a bldgs:hasPhysicalLocation ?EsLocation.
 ?b bldgs:hasPhysicalLocation ?bLocation.
 OPTIONAL
 {
  ?b bldgs:hasPoint ?point.
  ?point bldgs:hasPhysicalLocation ?pointLocation.
 }
}
```
Copy the following in your `cmd.exe` window:
```
stardog query myDbTest --reasoning "select ?a ?b ?point ?EsLocation ?bLocation ?pointLocation { ?a bldgs:connectsTo ?b.  ?a a bldgs:EnterpriseServer.  ?a bldgs:hasPhysicalLocation ?EsLocation.  ?b bldgs:hasPhysicalLocation ?bLocation.  OPTIONAL  {  ?b bldgs:hasPoint ?point.   ?point   bldgs:hasPhysicalLocation ?pointLocation. } }"
```

### v. Add The Point's Type and Unit
------------------------------------
```sql
select ?a ?b ?point ?EsLocation ?bLocation ?pointLocation ?typeOfPoint ?unitOfPoint 
{
 ?a bldgs:connectsTo ?b.
 ?a a bldgs:EnterpriseServer.
 ?a bldgs:hasPhysicalLocation ?EsLocation.
 ?b bldgs:hasPhysicalLocation ?bLocation.
 OPTIONAL
 {
  ?b bldgs:hasPoint ?point.
  ?point bldgs:hasPhysicalLocation ?pointLocation.
  ?point bldgs:hasType ?typeOfPoint.
  ?point bldgs:hasUnit ?unitOfPoint.
 }
}
```


Copy the following in your `cmd.exe` window:
```
stardog query myDbTest --reasoning "select ?a ?b ?point ?EsLocation ?bLocation ?pointLocation ?typeOfPoint ?unitOfPoint  {  ?a bldgs:connectsTo ?b.  ?a a bldgs:EnterpriseServer.  ?a bldgs:hasPhysicalLocation ?EsLocation.  ?b bldgs:hasPhysicalLocation ?bLocation.  OPTIONAL  {    ?b bldgs:hasPoint ?point.   ?point bldgs:hasPhysicalLocation ?pointLocation.   ?point bldgs:hasType ?typeOfPoint.   ?point bldgs:hasUnit ?unitOfPoint.  } }"
```

### vi. Add Node's Type
-----------------------
```sql
select ?a ?b ?point ?EsLocation ?bLocation ?pointLocation ?typeOfPoint ?unitOfPoint ?typeOfB
{
 ?a bldgs:connectsTo ?b.
 ?a a bldgs:EnterpriseServer.
 ?b a ?typeOfB.
 ?a bldgs:hasPhysicalLocation ?EsLocation.
 ?b bldgs:hasPhysicalLocation ?bLocation.
 OPTIONAL
 {
  ?b bldgs:hasPoint ?point.
  ?point bldgs:hasPhysicalLocation ?pointLocation.
  ?point bldgs:hasType ?typeOfPoint.
  ?point bldgs:hasUnit ?unitOfPoint.
 }
}
```

Copy the following in your `cmd.exe` window:
```
stardog query myDbTest --reasoning "select ?a ?b ?point ?EsLocation ?bLocation ?pointLocation ?typeOfPoint ?unitOfPoint ?typeOfB {  ?a bldgs:connectsTo ?b.  ?a a bldgs:EnterpriseServer.  ?b a ?typeOfB.  ?a bldgs:hasPhysicalLocation ?EsLocation.  ?b bldgs:hasPhysicalLocation ?bLocation.  OPTIONAL  {   ?b bldgs:hasPoint ?point.   ?point bldgs:hasPhysicalLocation ?pointLocation.   ?point bldgs:hasType ?typeOfPoint.   ?point bldgs:hasUnit ?unitOfPoint.  } }"
```

### vii. Filter The Node and Thing
----------------------------------
```sql
select ?a ?b ?point ?EsLocation ?bLocation ?pointLocation ?typeOfB ?typeOfPoint ?unitOfPoint
{
 ?a bldgs:connectsTo ?b.
 ?a a bldgs:EnterpriseServer.
 ?b a ?typeOfB.
 ?a bldgs:hasPhysicalLocation ?EsLocation.
 ?b bldgs:hasPhysicalLocation ?bLocation.
 OPTIONAL
 {
  ?b bldgs:hasPoint ?point.
  ?point bldgs:hasPhysicalLocation ?pointLocation.
  ?point bldgs:hasType ?typeOfPoint.
  ?point bldgs:hasUnit ?unitOfPoint.
 }
 Filter( ?typeOfB != bldgs:Node && ?typeOfB != owl:Thing)
}
```
In this step, you formulated queries and you needed to learn part of the SPARQL syntax. We are working on a Visual Query Builder where you can discover and query your topology in a dynamic and a visual tool as shown in the coming soon section [12](#visual-query-builder)

These examples, showed a little aspect of the reasoning regarding transititivity. Other properties can also be usefull in the Internet Of Things domain. The [Pizza tutorial](http://mowl-power.cs.man.ac.uk/protegeowltutorial/resources/ProtegeOWLTutorialP4_v1_3.pdf) covers a large set of the reasoning.


# 10 Configuring a Persistent Store

Five steps are necessary to configure the entity context with a persistent store such as _Stardog_.
1. Add the following in your **<configuration>** tag located in the App.config file of your .Net project:
```xml
  <romanticWeb.dotNetRDF>
      <stores>
      <persistent name="myStardogConfiguration">
        <customProvider type="VDS.RDF.Storage.StardogConnector, dotNetRDF">
          <parameters>
            <add key="baseUri" value="http://localhost:5820/"/>
            <add key="kbID" value="myDbTest"/>
            <add key="username" value="admin"/>
            <add key="password" value="admin"/>
          </parameters>
        </customProvider>
      </persistent>
    </stores>
  </romanticWeb.dotNetRDF>
```

2. [Create a database](#7-create-an-rdf-database) according to the name chosen in the previous configuration:
```
 <add key="kbID" value="myDbTest"/>
```

3. Add the confiSections in your **<configuration>** tag:
```xml
<configSections>
    <section name="romanticWeb" type="RomanticWeb.Configuration.ConfigurationSectionHandler, RomanticWeb"/>
    <section name="romanticWeb.dotNetRDF" type="RomanticWeb.DotNetRDF.Configuration.StoresConfigurationSection, RomanticWeb.DotNetRDF"/>
  </configSections>
```

4. Create your entity context with the persistent store configuration
```C#
   public static void InitContextWithStardog()
     {
       var contextFactory = new EntityContextFactory();
       contextFactory.WithMappings((MappingBuilder builder) => {...});
            
       store = StoresConfigurationSection.Default.CreateStore("myStardogConfiguration");
       contextFactory.WithDotNetRDF(store);
       contextFactory.WithMetaGraphUri(new Uri("http://www.schneider-electric.com/Buildings/Lund"));
       context = contextFactory.CreateContext();
     }
```

# 11 Next Steps
In this session, we created simple concepts to illustrate the usage of the ontologies and the queries. The next steps can be to rely on an existing standard where such classes are defined such as [SAREF 4 Buildings](http://ontoology.linkeddata.es/publish/saref4bldg/index-en.html), [Project HayStack](http://www.project-haystack.org/), [Brick for Buildings](http://brickschema.org/), or others.

# Suggestions or Bugs? Please reach out
Report it [here](https://github.com/InnovationSE/Ontology-Workshop/issues)
or write to us at: charbel.kaed@outlook.com
