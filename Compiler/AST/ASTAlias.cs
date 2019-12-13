﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Compiler.AST
{
    public class ASTAlias : IASTNode, IRestrictable, IElement, INamable, IRootNode, ICloneable
    {
        public string Name { get; } = "";
        public string Module { get; }
        public IEnumerable<ASTTypeDefinition> Type { get; } = Enumerable.Empty<ASTTypeDefinition>();
        public IEnumerable<ASTRestriction> Restrictions { get; } = Enumerable.Empty<ASTRestriction>();
        public IEnumerable<ASTAnnotation> Annotations { get; } = Enumerable.Empty<ASTAnnotation>();
        public IEnumerable<ASTDirective> Directives { get; } = Enumerable.Empty<ASTDirective>();

        public ASTAlias(
            string name,
            string module,
            IEnumerable<ASTTypeDefinition> types,
            IEnumerable<ASTRestriction> restrictions,
            IEnumerable<ASTAnnotation> annotations,
            IEnumerable<ASTDirective> directives) {
            this.Name = name;
            this.Type = types;
            this.Restrictions = restrictions;
            this.Annotations = annotations;
            this.Directives = directives;
            this.Module = module;
        }

        public static (List<ASTError>, ASTAlias) Parse(
                IParser parser,
                IEnumerable<ASTAnnotation> annotations,
                IEnumerable<ASTDirective> directives,
                string module = "")
        {
            var errors = new List<ASTError>();
            
            if (parser.HasNext()) parser.Next();
            var nameId = parser.Consume(TokenType.Identifier);
            parser.Consume(TokenType.Equal);
            var types = ASTTypeDefinition.Parse(parser, module).ToList();
            var restrictions = ASTRestriction.CreateRestrictions(parser, TokenType.KW_Alias).ToList();
            parser.TryConsume(TokenType.EndStatement);
            parser.Consume(TokenType.ContextEnded);

            var result = new ASTAlias(
                nameId.Value,
                module,
                types,
                restrictions,
                annotations,
                directives);

            return (errors, result);
        }

        public object Clone()
        {
            return new ASTAlias(
                (string)this.Name.Clone(),
                (string)this.Module.Clone(),
                ObjectCloner.CloneList(this.Type.ToList()),
                ObjectCloner.CloneList(this.Restrictions.ToList()),
                ObjectCloner.CloneList(this.Annotations.ToList()),
                ObjectCloner.CloneList(this.Directives.ToList())
            );
        }
    }
}
