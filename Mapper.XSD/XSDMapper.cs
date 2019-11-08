﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using Compiler;
using Compiler.AST;

namespace Mapper.XSD
{
    public class XSDMapper : VisitorBase<XmlSchemaObject?>
    {
        private const string DefaultSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

        public XmlSchema Schema { get; } = new XmlSchema();

        public XSDMapper(IEnumerable<IASTNode> nodeTree) : base(nodeTree)
        {
            Schema.Namespaces.Add("self", "org.schema.something");
        }

        public override XmlSchemaObject VisitASTType(ASTType astType)
        {
            XmlSchemaComplexType t = new XmlSchemaComplexType
            {
                Name = astType.Name
            };

            var fields = astType.Fields.Select(Visit);
            XmlSchemaAll all = new XmlSchemaAll();
            foreach (var field in fields)
            {
                all.Items.Add(field);
            }

            t.Particle = all;

            var description = string.Join(" ", astType.Annotations.Select(a => a.Value));
            XmlSchemaAnnotation schemaAnnotation = new XmlSchemaAnnotation();
            XmlSchemaDocumentation docs = new XmlSchemaDocumentation()
            {
                Markup = TextToNodeArray(description)
            };
            schemaAnnotation.Items.Add(docs);
            t.Annotation = schemaAnnotation;

            this.Schema.Items.Add(t);

            ExtractElement(astType);

            return t;
        }

        public override XmlSchemaObject VisitASTAlias(ASTAlias astAlias)
        {
            string _modifier = astAlias.Type.First().Value;
            string _type = astAlias.Type.Last().Value;
            
            if (_modifier == "List")
            {
                var list = Mapper.MapList(astAlias);
                Schema.Items.Add(list);
                return list;
            }

            var result = _type switch
            {
                "String" => Mapper.MapString(astAlias),
                "Number" => Mapper.MapNumber(astAlias),
                _ => Mapper.MapString(astAlias),
            };
            Schema.Items.Add(result);
            ExtractElement(astAlias);

            return result;
        }

        public override XmlSchemaObject VisitASTChoice(ASTChoice astChoice)
        {
            XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
            xmlSchemaComplexType.Name = astChoice.Name;

            XmlSchemaChoice xmlSchemaChoice = new XmlSchemaChoice();

            foreach (ASTOption option in astChoice.Options)
            {
                XmlSchemaElement o = new XmlSchemaElement
                {
                    Name = option.Value,
                    SchemaTypeName = new System.Xml.XmlQualifiedName("string", DefaultSchemaNamespace)
                };
                xmlSchemaChoice.Items.Add(o);
            }

            xmlSchemaComplexType.Particle = xmlSchemaChoice;
            this.Schema.Items.Add(xmlSchemaComplexType);
            return xmlSchemaComplexType;
        }

        public override XmlSchemaObject? VisitASTAnnotation(ASTAnnotation astAnnotation)
        {
            return null;
        }

        public override XmlSchemaObject? VisitASTDirective(ASTDirective astDirective)
        {
            return null;
        }

        public override XmlSchemaObject? VisitASTTypeDefinition(ASTTypeDefinition astTypeDefinition)
        {
            return null;
        }

        public override XmlSchemaObject? VisitASTTypeField(ASTTypeField astTypeField)
        {
            return Mapper.Element(astTypeField);
        }

        public override XmlSchemaObject? VisitASTRestriction(ASTRestriction astRestriction)
        {
            return null;
        }

        public override XmlSchemaObject? VisitASTOption(ASTOption astOption)
        {
            return null;
        }

        public override XmlSchemaObject? VisitDefault(IASTNode node)
        {
            return null;
        }

        public override XmlSchemaObject? VisitASTChapter(ASTChapter astOption)
        {
            return null;
        }

        public override XmlSchemaObject? VisitASTParagraph(ASTParagraph astOption)
        {
            return null;
        }

        private void ExtractElement<T>(T node) where T : INamable, IRootNode
        {
            var xsdDirective = node.Directives.FirstOrDefault(d => d.Key == "xsd");
            if (!(xsdDirective is null))
            {
                XmlSchemaElement element = new XmlSchemaElement();
                element.Name = xsdDirective.Value.Replace(" ", "_");
                element.RefName = new System.Xml.XmlQualifiedName("self:" + (node as INamable).Name);
                Schema.Items.Add(element);
            }
        }
        private XmlNode[] TextToNodeArray(string text)
        {
            XmlDocument doc = new XmlDocument();
            return new XmlNode[1] { doc.CreateTextNode(text) };
        }

    }
}

/*
<xs:complexType>
    <xs:choice>
      <xs:element name="employee" type="employee"/>
      <xs:element name="member" type="member"/>
    </xs:choice>
  </xs:complexType>
*/
