/****************************************************************************
 * Copyright (c) 2017 xiaojun
 * Copyright (c) 2017 imagicbell
 * Copyright (c) 2017 ~ 2021.3  liangxie
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System.Linq;
using UnityEngine;

namespace QFramework
{
    [MonoSingletonPath("UIRoot/Manager")]
    public partial class UIManager : QMgrBehaviour, ISingleton
    {
        void ISingleton.OnSingletonInit()
        {
        }

        private static UIManager mInstance;

        public static UIManager Instance
        {
            get
            {
                if (!mInstance)
                {
                    var uiRoot = UIRoot.Instance;
                    Debug.Log("currentUIRoot:" + uiRoot);
                    mInstance = MonoSingletonProperty<UIManager>.Instance;
                }

                return mInstance;
            }
        }

        public IPanel OpenUI(PanelSearchKeys panelSearchKeys)
        {
            if (panelSearchKeys.OpenType == PanelOpenType.Single)
            {
                var retPanel = XUIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

                if (retPanel == null)
                {
                    retPanel = CreateUI(panelSearchKeys);
                }

                retPanel.Open(panelSearchKeys.UIData);
                retPanel.Show();
                return retPanel;
            }
            else
            {
                var retPanel = CreateUI(panelSearchKeys);
                retPanel.Open(panelSearchKeys.UIData);
                retPanel.Show();
                return retPanel;
            }
        }

        public void OpenUI(PanelSearchKeys panelSearchKeys,System.Action<IPanel> OnCompleted)
        {
            if (panelSearchKeys.OpenType == PanelOpenType.Single)
            {
                var retPanel = XUIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

                if (retPanel == null)
                {
                    CreateUI(panelSearchKeys,(_IPanel)=> {
                        _IPanel.Open(panelSearchKeys.UIData);
                        _IPanel.Show();
                        OnCompleted(_IPanel);
                    });
                }
            }
            else
            {
                CreateUI(panelSearchKeys, (_IPanel) => {
                    _IPanel.Open(panelSearchKeys.UIData);
                    _IPanel.Show();
                    OnCompleted(_IPanel);
                });
            }
        }

        /// <summary>
        /// 显示UIBehaiviour
        /// </summary>
        /// <param name="uiBehaviourName"></param>
        public void ShowUI(PanelSearchKeys panelSearchKeys)
        {
            var retPanel = XUIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

            if (retPanel != null)
            {
                retPanel.Show();
            }
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="uiBehaviourName"></param>
        public void HideUI(PanelSearchKeys panelSearchKeys)
        {
            var retPanel = XUIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

            if (retPanel != null)
            {
                retPanel.Hide();
            }
        }

        /// <summary>
        /// 删除所有UI层
        /// </summary>
        public void CloseAllUI()
        {
            foreach (var layer in XUIKit.Table)
            {
                layer.Close();
                layer.Info.Recycle2Cache();
                layer.Info = null;
            }

            XUIKit.Table.Clear();
        }

        /// <summary>
        /// 隐藏所有 UI
        /// </summary>
        public void HideAllUI()
        {
            foreach (var panel in XUIKit.Table)
            {
                panel.Hide();
            }
        }

        /// <summary>
        /// 关闭并卸载UI
        /// </summary>
        /// <param name="behaviourName"></param>
        public void CloseUI(PanelSearchKeys panelSearchKeys)
        {
            var panel = XUIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).LastOrDefault();

            if (panel as UIPanel)
            {
                panel.Close();
                XUIKit.Table.Remove(panel);
                panel.Info.Recycle2Cache();
                panel.Info = null;
            }
        }

        public void RemoveUI(PanelSearchKeys panelSearchKeys)
        {
            var panel = XUIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

            if (panel != null)
            {
                XUIKit.Table.Remove(panel);
            }
        }

        /// <summary>
        /// 获取UIBehaviour
        /// </summary>
        /// <param name="uiBehaviourName"></param>
        /// <returns></returns>
        public UIPanel GetUI(PanelSearchKeys panelSearchKeys)
        {
            return XUIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault() as UIPanel;
        }

        public override int ManagerId
        {
            get { return QMgrID.UI; }
        }

        public IPanel CreateUI(PanelSearchKeys panelSearchKeys)
        {
            var panel = XUIKit.Config.LoadPanel(panelSearchKeys);

            XUIKit.Root.SetLevelOfPanel(panelSearchKeys.Level, panel);

            XUIKit.Config.SetDefaultSizeOfPanel(panel);

            panel.Transform.gameObject.name = panelSearchKeys.GameObjName ?? panelSearchKeys.PanelType.Name;

            panel.Info = PanelInfo.Allocate(panelSearchKeys.GameObjName, panelSearchKeys.Level, panelSearchKeys.UIData,
                panelSearchKeys.PanelType, panelSearchKeys.AssetBundleName);
            
            XUIKit.Table.Add(panel);

            panel.Init(panelSearchKeys.UIData);

            return panel;
        }

        public void CreateUI(PanelSearchKeys panelSearchKeys,System.Action<IPanel> OnCompleted)
        {
            XUIKit.Config.LoadPanelAsync(panelSearchKeys,(panel)=> {

                XUIKit.Root.SetLevelOfPanel(panelSearchKeys.Level, panel);

                XUIKit.Config.SetDefaultSizeOfPanel(panel);

                panel.Transform.gameObject.name = panelSearchKeys.GameObjName ?? panelSearchKeys.PanelType.Name;

                panel.Info = PanelInfo.Allocate(panelSearchKeys.GameObjName, panelSearchKeys.Level, panelSearchKeys.UIData,
                    panelSearchKeys.PanelType, panelSearchKeys.AssetBundleName);

                XUIKit.Table.Add(panel);

                panel.Init(panelSearchKeys.UIData);

                OnCompleted(panel);

            });
        }
    }
}