// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.CSharp.RuntimeBinder
{
    /// <summary>
    /// Represents a dynamic unary operation in C#, providing the binding semantics and the details about the operation. 
    /// Instances of this class are generated by the C# compiler.
    /// </summary>
    internal sealed class CSharpUnaryOperationBinder : UnaryOperationBinder
    {
        internal bool IsChecked { get { return _isChecked; } }
        private bool _isChecked;

        internal Type CallingContext { get { return _callingContext; } }
        private Type _callingContext;

        internal IList<CSharpArgumentInfo> ArgumentInfo { get { return _argumentInfo.AsReadOnly(); } }
        private List<CSharpArgumentInfo> _argumentInfo;

        private RuntimeBinder _binder;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpUnaryOperationBinder"/> class.
        /// </summary>
        /// <param name="operation">The unary operation kind.</param>
        /// <param name="isChecked">True if the operation is defined in a checked context; otherwise, false.</param>
        /// <param name="argumentInfo">The sequence of <see cref="CSharpArgumentInfo"/> instances for the arguments to this operation.</param>
        public CSharpUnaryOperationBinder(
            ExpressionType operation,
            bool isChecked,
            Type callingContext,
            IEnumerable<CSharpArgumentInfo> argumentInfo) :
            base(operation)
        {
            _isChecked = isChecked;
            _callingContext = callingContext;
            _argumentInfo = BinderHelper.ToList(argumentInfo);
            Debug.Assert(_argumentInfo.Count == 1);
            _binder = RuntimeBinder.GetInstance();
        }

        /// <summary>
        /// Performs the binding of the unary dynamic operation if the target dynamic object cannot bind.
        /// </summary>
        /// <param name="target">The target of the dynamic unary operation.</param>
        /// <param name="errorSuggestion">The binding result in case the binding fails, or null.</param>
        /// <returns>The <see cref="DynamicMetaObject"/> representing the result of the binding.</returns>
        public sealed override DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            return BinderHelper.Bind(this, _binder, BinderHelper.Cons(target, null), _argumentInfo, errorSuggestion);
        }
    }
}