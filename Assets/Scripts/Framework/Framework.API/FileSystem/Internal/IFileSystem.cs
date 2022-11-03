﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.IO;

namespace CatLib.API.FileSystem
{
    /// <summary>
    /// 文件系统
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// 文件或文件夹是否存在
        /// </summary>
        /// <param name="path">文件或文件夹是否存在</param>
        /// <returns>是否存在</returns>
        bool Exists(string path);

        /// <summary>
        /// 写入数据
        /// 如果数据已经存在则覆盖
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="contents">写入数据</param>
        void Write(string path, byte[] contents);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>读取的数据</returns>
        byte[] Read(string path);

        /// <summary>
        /// 移动文件到指定目录
        /// </summary>
        /// <param name="path">旧的文件/文件夹路径</param>
        /// <param name="newPath">新的文件/文件夹路径</param>
        void Move(string path, string newPath);

        /// <summary>
        /// 复制文件或文件夹到指定路径
        /// </summary>
        /// <param name="path">文件或文件夹路径(应该包含文件夹或者文件名)</param>
        /// <param name="copyPath">复制到的路径(不应该包含文件夹或者文件名)</param>
        void Copy(string path, string copyPath);

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        /// <param name="path">路径</param>
        void Delete(string path);

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        void MakeDir(string path);

        /// <summary>
        /// 获取文件/文件夹句柄
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹句柄</returns>
        T GetHandler<T>(string path) where T : class, IHandler;

        /// <summary>
        /// 获取文件/文件夹的大小(字节)
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹的大小</returns>
        long GetSize(string path = null);

        /// <summary>
        /// 获取文件/文件夹属性
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹属性</returns>
        FileAttributes GetAttributes(string path);

        /// <summary>
        /// 获取列表（不会迭代子文件夹）
        /// </summary>
        /// <param name="path">要获取列表的文件夹路径(如果传入的是一个文件那么将会返回文件自身路径)</param>
        /// <returns>指定目录下的文件夹句柄和文件句柄列表</returns>
        IHandler[] GetList(string path = null);
    }
}
