using RomanticWeb;
using RomanticWeb.DotNetRDF;
using RomanticWeb.Entities;
using RomanticWeb.Mapping;
using RomanticWeb.Ontologies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Writing;

namespace BMSOnt
{
    class Program
    {
        static IEntityContext context;
        static ITripleStore store;
        static string clientURI = "http://www.schneider-electric.com/Buildings/Lund";

        static void Main(string[] args)
        {
            InitContext();
            CreateThingsTopology();

            IGraph g = new Graph();
            IList graphList = store.Graphs.ToList();

            foreach (IGraph currentGraph in graphList)
            {
                g.Merge(currentGraph);
            }

            RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
            rdfxmlwriter.Save(g, "C:\\output.rdf");
        }

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

        public static void CreateThingsTopology()
        {
            IPoint p1 = context.Create<IPoint>(new Uri(clientURI + "#temp1"));
            p1.Name = "Point1";
            p1.UID = "point1-111";
            p1.Unit = "DegreeC";
            p1.Type = "Temperature";

            IPoint p2 = context.Create<IPoint>(new Uri(clientURI + "#temp2"));
            p2.Name = "Point2";
            p2.UID = "point2-222";
            p2.Unit = "DegreeC";
            p2.Type = "Temperature";

            IController c1 = context.Create<IController>(new Uri(clientURI + "#c1"));
            c1.Name = "Controller1";
            c1.UID = "Controller1-111";

            c1.HasPoints.Add(p1);
            c1.HasPoints.Add(p2);

            //==========================//

            IPoint p3 = context.Create<IPoint>(new Uri(clientURI + "#energy3"));
            p3.Name = "Point3";
            p3.UID = "point3-333";
            p3.Unit = "Wh";
            p3.Type = "Energy";

            IPoint p4 = context.Create<IPoint>(new Uri(clientURI + "#energy4"));
            p4.Name = "Point4";
            p4.UID = "point4-444";
            p4.Unit = "Wh";
            p4.Type = "Energy";

            IController c2 = context.Create<IController>(new Uri(clientURI + "#c2"));
            c2.Name = "Controller2";
            c2.UID = "Controller2-222";
            c2.HasPoints.Add(p3);
            c2.HasPoints.Add(p4);

            //==========================//

            IPoint p5 = context.Create<IPoint>(new Uri(clientURI + "#temp5"));
            p5.Name = "Point5";
            p5.UID = "point5-555";
            p5.Unit = "DegreeC";
            p5.Type = "Temperature";

            IPoint p6 = context.Create<IPoint>(new Uri(clientURI + "#temp6"));
            p6.Name = "Point6";
            p6.Unit = "DegreeC";
            p6.Type = "Temperature";

            IController c3 = context.Create<IController>(new Uri(clientURI + "#c3"));
            c3.Name = "Controller3";
            c3.UID = "Controller3-333";
            c3.HasPoints.Add(p5);
            c3.HasPoints.Add(p6);

            //==========================//

            IPoint p7 = context.Create<IPoint>(new Uri(clientURI + "#energy7"));
            p7.Name = "Point7";
            p7.UID = "point7-777";
            p7.Unit = "Wh";
            p7.Type = "Energy";

            IPoint p8 = context.Create<IPoint>(new Uri(clientURI + "#energy8"));
            p8.Name = "Point8";
            p8.UID = "point8-888";
            p8.Unit = "Wh";
            p8.Type = "Energy";

            IController c4 = context.Create<IController>(new Uri(clientURI + "#c4"));
            c4.Name = "Controller4";
            c4.UID = "Controller4-444";
            c4.HasPoints.Add(p7);
            c4.HasPoints.Add(p8);

            //==========================//

            IAS as1 = context.Create<IAS>(new Uri(clientURI + "#as1"));
            as1.Name = "AutomationServer1";
            as1.UID = "AutomationServer1-123";
            as1.Controls.Add(c1);
            as1.Controls.Add(c2);

            IAS as2 = context.Create<IAS>(new Uri(clientURI + "#as2"));
            as2.Name = "AutomationServer2";
            as2.UID = "AutomationServer2-123";
            as2.Controls.Add(c3);
            as2.Controls.Add(c4);

            IES es = context.Create<IES>(new Uri(clientURI + "#es1"));
            es.Name = "EnterpriseServer1";
            es.UID = "EnterpriseServer1-123";
            es.Monitors.Add(as1);
            es.Monitors.Add(as2);

            // commit data to store
            context.Commit();
        }
    }
}