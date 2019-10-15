﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    /// <summary>
    /// Represents an input for parsing.
    /// </summary>
    public interface IInput : IEquatable<IInput>
    {
        /// <summary>
        /// Advances the input.
        /// </summary>
        /// <returns>A new <see cref="IInput" /> that is advanced.</returns>
        /// <exception cref="System.InvalidOperationException">The input is already at the end of the source.</exception>
        IInput Next();

        /// <summary>
        /// Gets the whole source.
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Gets the current <see cref="System.Char" />.
        /// </summary>
        char Current { get; }

        /// <summary>
        /// Gets a value indicating whether the end of the source is reached.
        /// </summary>
        bool AtEnd { get; }

        /// <summary>
        /// Gets the current positon.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Gets the current line number.
        /// </summary>
        int Line { get; }

        /// <summary>
        /// Gets the current column.
        /// </summary>
        int Column { get; }

        TakeWhile TakeWhile(Predicate<char> predicate);

        /// <summary>
        /// Memos used by this input
        /// </summary>
        IDictionary<object, object> Memos { get; }
    }
}