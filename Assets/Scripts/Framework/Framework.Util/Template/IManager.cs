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

namespace CatLib
{
    /// <summary>
    /// 管理器
    /// </summary>
    public interface IManager<TInterface> : IManaged<TInterface>
    {
        /// <summary>
        /// 获取扩展实现
        /// </summary>
        /// <param name="name">扩展名</param>
        /// <returns>扩展实现</returns>
        TInterface Get(string name = null);

        /// <summary>
        /// 获取扩展实现
        /// </summary>
        /// <param name="name">扩展名</param>
        /// <returns>扩展实现</returns>
        TInterface this[string name] { get; }
    }
}
