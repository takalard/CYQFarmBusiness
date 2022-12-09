using UnityEngine;
using System.Collections;

public abstract class ItemAbstract
{
    public int index;
    public int typeShow = 0;//0 Target level, 1 tartget Number  . Trong Data shop thì 0 là product, 1 là sell
    public int currentNumber;
    public int currentLevel;

    public int getCurrent()
    {
        if (typeShow == 0)
        {
            //Ở target common thì currentLevel = số sao hiện tại, currentNumber = số tiền hiện tại
            return currentLevel;
        }
        else
        {
            return currentNumber;
        }
    }

    public abstract int getTarget();
    //Loai cua object vi du ruong, ao..., Cai nay khac voi typeShow
    public abstract int getType();
}
