# _Protégé_ Session: Building A Model

## Disclaimer! 

In this guide, we use the famous open source _Protégé_ tool to give handson practice to start designing ontologies. However, it must be noted that we are not trying to address all the issues that an ontology developer may face. 
Instead, we try to provide a starting point; a first step to help a new ontology designer to develop ontologies. Also, there is no single correct ontology-design methodology and we did not attempt to define one. The steps presented here are typically used in an ontology development process.

## Table of Contents

- [Prerequisite](#prerequisite)
- [What is an Ontology and why develop it?](#what-is-an-ontology-and-why-develop-it?)
- [Protégé IDE](#protégé-ide)

1. [Create a Buildings Ontology](#1-create-a-buildings-ontology)
2. [Add some Classes](#2-add-some-classes)
3. [Extend the Ontology](#3-extend-the-ontology)
4. [Specify Characteristics of these Classes](#4-specify-characteristics-of-these-classes)
5. [Create Relationships Between Classes](#5-create-relationships-between-classes)
6. [Create Individuals of Classes](#6-create-individuals-of-classes)
7. [Additional Classes](#7-additional-classes)
8. [Create more Relationships](#8-create-more-relationships)
9. [Exercise](#9-exercise) 
10. [Graphical View of Ontology](#10-graphical-view-of-ontology)
11. [Reasoning](#11-reasoning)
12. [Next Session](#12-next-session)


## Prerequisite
1. A _Java Virtual Machine_ (version > 1.8) [download JVM 1.8](http://www.oracle.com/technetwork/java/javase/downloads/jre8-downloads-2133155.html)
2. _Protégé_ is an open source ontology editor provided by Stanford University. [download Protégé](http://protege.stanford.edu/products.php#desktop-protege).


## What is an Ontology and why develop it?

To refresh the concepts, _an ontology defines a common vocabulary for users who need to share information in a given domain. It includes machine-interpretable definitions of basic concepts in the domain and relations among them. In addition, an ontology offers a rich structure to represent information, and more precisely an Internet of Things (IoT) system and installation_.

Why would someone want to develop an ontology? Some of the reasons are:

  - To share common understanding of the structure of information among people or software agents
  - To enable reuse of domain knowledge
  - To make domain assumptions explicit
  - To separate domain knowledge from the operational knowledge
  - To analyze domain knowledge
  - To represent an IoT system's topology in a very expressive structure


## _Protégé_ IDE

When you start _Protégé_, you will get a screen similar to the figure below.

![Protege-opening-screen](/protege-session/images/1.JPG)

In the begining it will be empty and _Protégé_ will assign random __Ontology IRI__ and __Ontology Version IRI__. You can change them to suit your needs.

Using the Windows menu on the toolbar, you can activate different tabs in the IDE. In this guide we will activate the following tabs as shown in the figure below:
1. _Active Ontology_
2. _Entities_, _Classes_
3. _Object Properties_
4. _Data Properties_
5. _Individuals by class_

![Protege-tabs](/protege-session/images/2.JPG)

## 1 Create a Buildings Ontology

Let's create our example ontology for the buildings domain. Click on the __Active Ontology Tab__. 

First, let's give our custom __Ontology IRI__ in the field `http://www.schneider-electric.com/Buildings`.

![Protege-IRI](/protege-session/images/3.JPG)

Now you can include some information for this new Ontology as annotations, like comments, versionInfo, isDefinedBy and so on. 

Click on the __+ sign__ next to the greyed Annotations heading the panel below to add some annotations as shown in the figure below.

For this example we added:
1. anotations _rdfs:comment_ `This is a test ontology for Buildings`

2. _rdfs:isDefinedBy_ `Buildings team` as annotations.

![Protege-ontology-comments](/protege-session/images/4.JPG)

## 2 Add some Classes

Now lets add some concepts to our ontology. 
1. Click on __Classes tab__. 
2. On the left slide click on __Class hierarchy__ tab. you will notice _owl:Thing_ as the default class already existing. In the Ontology world all classes are subclass of the class _owl:Thing_. 
3. Select owl:Thing in the __Class hierarchy__ tab and add a subclass _Location_ as shown below.
![Protege-add-location](/protege-session/images/5.JPG)
4. Select the newly created _Location_ class and add few more subclasses by reapting the same previous steps. Add subclasses _Building_, _Floor_, _Room_ as 3 subclasses of _Location_ class as shown below.

![Protege-3-classes](/protege-session/images/6.JPG)

## 3 Extend the Ontology
You can extend the ontology with few other classes  related to the *Location* class in your domain. For example, _Site_, _Area_, _Open-Space_, or others.? If yes then create them as subclases of Location class repeating the same steps as above.

## 4 Specify Characteristics of these Classes

After adding these classes, we declare them as _disjoint_ this information is used by the reasoner.
 
1. Select the class _Building_
2. Click on the _+ sign_ in front of the _Disjoint With_ label in the _Description_ view at the bottom, as show in the figure below.

![Protege-disjoint](/protege-session/images/7.JPG)

3. Select all classes that are disjoint with the _Building_ class. You can use _Ctrl + Left Click_ to select multiple classes at the same time. Add _Floor_ and _Room_ 
4. Click OK,  the output of this step is shown in the figure below. 

![Protege-disjoint-final](/protege-session/images/8.JPG)

_Protégé_ already added to the _Floor_ and _Room_ classes the disjoint with other classes automatically.

## 5 Create Relationships Between Classes

Next we create some OWL properties to represent relationships between our classes. There are two main types of properties, __Object Properties__ and __Datatype Properties__. 

* an Object Property links an object to another. Constraints and cardinality along with other properties can also be added. The [Pizza tutorial](http://mowl-power.cs.man.ac.uk/protegeowltutorial/resources/ProtegeOWLTutorialP4_v1_3.pdf) covers a large set of the object properties and possible usage.
* A Datatype property links an individual to the data literal such as _integer_, _boolean_, _string_.

In this example we will create an Object Property:
1. Click on __Object Properties__ tab. 
2. _owl:topObjectProperty_ is present already, select it and a new object property, _hasLocation_ as shown below.

![Protege-obj-prop](/protege-session/images/9.JPG)
3. After clicking OK, you can select different characteristics for this property. In this example, we select __Transitive__ as shown in the figure below.

![Protege-obj-prop-charac](/protege-session/images/10.JPG)

4. Let's create more __SubProperties__. Select _hasLocation_ property and using the same steps as above, create two _subProperties_ _hasLocationFloor_ and _hasLocationRoom_ as shown in the figure below.

![Protege-obj-prop-subProps](/protege-session/images/11.JPG)

Now let's improve our _Building_ and _Floor_ classes using the newly created object properties.

5. Click on the __Classes__ tabs and select _Building_ class. 
6. In the bottom panel _Description_ click on the __+ sign__ next to the _SubClassOf_ label. 
7. A dialog will pop up, click on the _object restriction creator_ tab.
8. Expand and select _hasLocationFloor_ from the Restricted property part and _Floor_ from the Restriction filter part, as show in the figure below.

![Protege-12](/protege-session/images/12.JPG)

Repeat the same steps for __Floor__ class and this time select _hasLocationRoom_ and _Room_ in the dialog box and click OK.

## 6 Create Individuals of Classes
Individuals are similar to the instances in the Object Oriented Programming. In this step, we will instantiate some individuals of the classes we created earlier.

<!--
For this we first need to enable __Individuals by class__ tab from the _Windows_ menu on the top. See figure below to do this.

![Protege-13](/protege-session/images/13.JPG)
-->

1. Select the __Individuals by class__ tab and select __Building__ in the left most panel. 
2. Click on the _add individual_ icon in the __Instances__ panel. A new dialog box will apear. 
3. Put any name in the empty field and click OK. In this example we put _BuildingT3_ as show in the figure below.

![Protege-14](/protege-session/images/14.JPG)

You can create several individuals using unique uri. In this example we created _BuildingT3_, _BuildingT4_, and _BuildingT11_.

![Protege-15](/protege-session/images/15.JPG)

4. Repeat the same steps for the __Floor__ and the __Room__ classes.

## 7 Additional Classes
So far, we created simple concepts in our Buildings ontology. We will add additional classes to make it more useful.

We will add the new classes as follows:

```
Quantity
	ElectricCurrrent
	Energy
		ActiveEnergy
		ApparentEnergy
		ReactiveEnergy
	Power
	Temperature
	Voltage
Node
	AutomationServer
	Controller
	EnterpriseServer
	Point
Unit
	Ampere
	Celsius
	Fahrenheit
	Volt
	Watt
	Watt-Hour
```

_Protégé_ allows creating a class hierarchy where several classes are declared at once, as presented in the following:

1. Copy the above code and in _Protégé_ 
2. Select _owl:Thing_ in the Class Hierarchy panel
3. Click on _Tools_ menu at the top 
4. Select _Create class hierarchy..._ as shown in figure below.

![Protege-16](/protege-session/images/16.JPG)

5. Paste the copied code in the dialog
6. Click on continue and take care of the indents as they are required to create hierarchy between classes

![Protege-17](/protege-session/images/17.JPG)

7. A confirmation screen with a checkbox to _make sibling classes disjoin_ will apear. Keep this option checked and click Finish. 

The output of this step is shown in the figure below.

![Protege-18](/protege-session/images/18.JPG)

## 8 Create more Relationships
In this step, we will create additional __Object Properties__ for the individuals of the new classes. We will define object properties and their inverse relations which will be used by the reasoner to infer new information later.

1. Click on the _Object Properties_ tab
2. Select __owl:topObjectProperty__
3. Click on the _Tools_ Menu on top
4. Select _Create object property hierarchy
5. A dialog box will appear where we will create the hierarchy of the object properties.
5. Copy the following code into the dialog box as shown in the figure below. Take care of the indents as they define the hierarchy.

```
controls
hasPoint
hasType
hasUnit
isControlledBy
```

![Protege-19](/protege-session/images/19.JPG)

6. Click on _Continue_ and then _Finish_ on next screen without ticking the checkbox.

7. Select _controls_ object property and tick __Transitive__ in the **Charactertistics panel**.

8. The Ontology Web Language allows to declare object properties and their inverse ones. Select _isConnectedTo_ and add its inverse Of _connectsTo__ as shown in the figure below.

![Protege-inverse](/protege-session/images/20.JPG)

## 9 Exercise
Before moving to the reasoning section, we need to go through the following:

## Inverse of hasLocation
Add the following object properties to _hasLocation_ : 
1. *isLocatedIn*, inverse Of *hasLocation*
2. *isLocatedInFloor*, inverse Of *hasLocationRoom*
3. *isLocatedInBuilding*, inverse Of *hasLocationFloor*

## Add Sub Object Properties of connectsTo
1. Create the object property _connectsTo_ under _owl:topObjectProperty_ 
2. Create sub Property of _connectsTo_ called _monitors_
3. Drag _controls_ property with your mouse and drop it on __connectsTo__  Both _monitors_ and _controls_ should be under _connectsTo_.

4. Add the Object Properties: _isConnectedTo_, __monitoredBy__, __controlledBy__ under _owl:topSubProperty_. 
5. Declare them inverseOf with _connectsTo_, _controls_, and _monitors_ Object Properties respectively created in Step 2.

## Link the Classes with their Properties
Create the following links between the classes:
1. _EnterpriseServer_ _monitors_ _AutomationServer_
2. _AutomationServer_ _controls_ _Controller_
3. _Controller_ _controls_ _Point_

## Instantiate the Nodes
Create the following instances by using the __Individuals by class__ tab:
1. _EnterpriseServer_ : ES1
2. _AutomationServer_ : AS1, AS2
3. _Controller_ : C1, C2

 Now connect these instances together as relationships defined in Step 4 above. 
 1. _ES1_ _monitors_ _AS1_ 
 2. _ES1_ _monitors_ _AS2_ 
 3. _AS1_ controls _C1_
 3. _AS2_ controls _C2_

## 10 Graphical View of Ontology

Download [VOWL Plugin for Protégé](http://vowl.visualdataweb.org/protegevowl.html) Jar file:

1. Add the Jar file to the following folder **$Protege\Plugin** 
2. Restart _Protégé_
3. Click on Menu Windows -> Tabs -> VOWL
4. Click on VOWL tab and see your ontology graphically as shown below

![Protege-21](/protege-session/images/21.JPG)

## 11 Reasoning
According to [[1]](https://en.wikipedia.org/wiki/Inference_engine), _an inference engine is a component of a system that applied logical rules to the knowledge base to deduce new information._
A semantic reasoner is a broader concept then an inference engine, it relies on wider and richer concepts to extract new information [[2]](https://en.wikipedia.org/wiki/Semantic_reasoner).

If you did not have time to complete the previous steps, you can download the following ontology [Buildings-Final.owl](/ontology/) for the reasoning part.

In this step we will add a new reasoner to _Protégé_ even though its is already donwloaded with some reasoners.
1. Download this [jar file](/plugin/) 
2. Add it to the following folder **$Protege\Plugin** 
3. Restart _Protégé_
4. Go to **Reasoner** and select **Pellet** as shown in the figure below
![](/protege-session/images/Pellet.png)
5. Go to **Reasoner** and select **Start reasoner** as  shown below
![](/protege-session/images/startReasoning.png)
6. Notice the following
![](/protege-session/images/reasoning.png)
The reasoner inferred the following:
```
Building1 hasLocation Room2
Building1 hasLocation Floor2
Building1 hasLocation Floor1
Building1 hasLocation Room1
```
Click on the **? sign** in the bottom right, the reasoner will explain the inference.

Similarly, you can notice the inference produced by the reasoner when you select the `EnterpriseServer` Class and its instance `ES`.

These examples, describe only one aspect of the reasoning which transititivity. Other properties can also be usefull in the Internet Of Things domain. The [Pizza tutorial](http://mowl-power.cs.man.ac.uk/protegeowltutorial/resources/ProtegeOWLTutorialP4_v1_3.pdf) covers a large set of the reasoning.

The next hands on session will focus on generating instances from a .Net library and formulating queries in memory and on an ontology store.

## 12 Next Session
In this session, we learned how to design and build an ontological model using _Protégé_. We went from defining a set of classes, properties and individuals and how they relate to each other. Then, we discovered the reasoner with a specific example applied to the transitivity. 

In the next section, we will go through the instantiation and generation of an ontology through programming without having any deep knowledge in the ontology world. We will focus on the software tools to transfer the ontology from the hands of the experts to the hands of an IoT and System developer.

[Go to the Next session](/coding-session/README.md)
