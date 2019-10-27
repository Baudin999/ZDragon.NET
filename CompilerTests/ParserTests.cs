﻿using System;
using Xunit;
using Compiler;
using System.Linq;
using Compiler.AST;
using System.Collections.Generic;

namespace CompilerTests
{
    public class ParserTests
    {
        [Fact]
        public void BasicParserTest()
        {
            var code = @"
type Person =
    @ The First Name of the Person
    FirstName: String;
";
            var tokens = new Lexer().Lex(code);
            var parseTree = new Parser(tokens).Parse();
            Assert.NotNull(parseTree);

            List<object> list = parseTree.ToList();
            Assert.Single(list);

            ASTType t = list[0] as ASTType;
            Assert.Equal("Person", t.Name);
            Assert.Empty(t.Parameters);
            Assert.Single(t.Fields);


            ASTTypeField field = t.Fields[0];
            Assert.Single(field.Annotations);
            Assert.True(field.Annotations[0] is ASTAnnotation);
            Assert.Equal("The First Name of the Person", field.Annotations[0].Value);

            Assert.Equal("FirstName", field.Name);
            Assert.Equal("String", field.Type);
        }

        [Fact]
        public void MultipleFieldsParserTest()
        {
            var code = @"
type Person =
    @ The First Name of the Person
    @ A second Annotation is always cool to add
    FirstName: String;
    LastName: String;
    @ Age as a number is weird ofc!
    Age: Number;
";
            var tokens = new Lexer().Lex(code);
            var parseTree = new Parser(tokens).Parse();
            Assert.NotNull(parseTree);

            List<object> list = parseTree.ToList();
            Assert.Single(list);

            ASTType t = list[0] as ASTType;
            Assert.Equal("Person", t.Name);
            Assert.Empty(t.Parameters);
            Assert.Equal(3, t.Fields.Count);

            ASTTypeField field = t.Fields[0];
            Assert.Equal(2, field.Annotations.Count);
            Assert.True(field.Annotations[0] is ASTAnnotation);
            Assert.Equal("The First Name of the Person", field.Annotations[0].Value);
            Assert.Equal("A second Annotation is always cool to add", field.Annotations[1].Value);

            Assert.Equal("FirstName", field.Name);
            Assert.Equal("String", field.Type);


            ASTTypeField lastNameField = t.Fields[1];
            Assert.Equal("LastName", lastNameField.Name);
            Assert.Equal("String", lastNameField.Type);
            Assert.Empty(lastNameField.Annotations);

            ASTTypeField ageField = t.Fields[2];
            Assert.Equal("Age", ageField.Name);
            Assert.Equal("Number", ageField.Type);
            Assert.Single(ageField.Annotations);
        }

        [Fact]
        public void MultipleTypes()
        {
            var code = @"
type Person =
    @ The First Name of the Person
    @ A second Annotation is always cool to add
    FirstName: String;
    LastName: String;
    @ Age as a number is weird ofc!
    Age: Number;
    Addresses: List Address;
type Address =
    Street: String;
    HouseNumber: Number;
";
            var tokens = new Lexer().Lex(code);
            var parseTree = new Parser(tokens).Parse().ToList();
            Assert.NotNull(parseTree);

            Assert.Equal(2, parseTree.Count());

            ASTType personType = parseTree[0] as ASTType;
            Assert.Equal("Person", personType.Name);

            ASTType addressType = parseTree[1] as ASTType;
            Assert.Equal("Address", addressType.Name);

        }

        [Fact]
        public void TypeAnnotations()
        {
            var code = @"
@ The person type
type Person
";
            var tokens = new Lexer().Lex(code);
            var parseTree = new Parser(tokens).Parse().ToList();
            Assert.NotNull(parseTree);

            Assert.Single(parseTree);
            Assert.Single((parseTree[0] as ASTType).Annotations);
        }
    }
}
