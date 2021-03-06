﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace CLI.Models
{
    public class LexiconEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = "";
        public string Domain { get; set; } = "";
        public string? FunctionalOwner { get; set; }
        public string? TechnicalOwner { get; set; }
        public string? Version { get; set; }

        public List<string> Applications { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
    }
}
