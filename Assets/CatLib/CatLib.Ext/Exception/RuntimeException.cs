﻿/*
 * This file is part of the CatLib package.
 *
 * (c) CatLib <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: https://catlib.io/
 */

using System;

namespace CatLib
{
    /// <summary>
    /// 运行时异常
    /// </summary>
    public class RuntimeException : System.Exception
    {
        /// <summary>
        /// 运行时异常
        /// </summary>
        public RuntimeException()
        {

        }

        /// <summary>
        /// 运行时异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public RuntimeException(string message) : base(message)
        {
        }

        /// <summary>
        /// 运行时异常
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public RuntimeException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}