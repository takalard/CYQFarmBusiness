using System;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class UIUtils
{
    public static void SetSprite(Image srcImage,string dstImageName)
    {
		// 同步加载图片
#if UNITY_WEBGL
		{
			AssetOperationHandle handle = YooAssets.LoadAssetAsync<Sprite>(dstImageName);
			//_cachedAssetOperationHandles.Add(handle);
			handle.Completed += (AssetOperationHandle obj) =>
			{
				srcImage.sprite = handle.AssetObject as Sprite;
			};
		}
#else
		{
			AssetOperationHandle handle = YooAssets.LoadAssetSync<Sprite>(dstImageName);
			//_cachedAssetOperationHandles.Add(handle);
			srcImage.sprite = handle.AssetObject as Sprite;
		}
#endif
	}
}
