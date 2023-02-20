using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{

    public static int GetMaxArrayElement(int[] array, out int index)
    {
        index = -1;
        int max = int.MinValue;
        for (int i = 0; i < array.Length; ++i)
        {
            if (array[i] > max)
            {
                max = array[i];
                index = i;
            }
        }
        return max;
    }

    //Returns highest three numbers highest being element 0 second being 1 and third being 2
    public static int[] GetMaxThreeArrayElement(int[] array)
    {
        int highestNum = 0;
        int secondHighestNum = 0;
        int thirdHighestNum = 0;

        int[] highestNumsIndex = { 0, 0, 0 };// pos 0 is highest, pos 1 is second highest, pos 2 is third heighest

        for(int i = 0; i < array.Length; i++)
        {
            //Get max val 
            if(array[i] > highestNum)
            {

                //Push numbers left
                thirdHighestNum = secondHighestNum;
                highestNumsIndex[2] = highestNumsIndex[1];

                secondHighestNum = highestNum;
                highestNumsIndex[1] = highestNumsIndex[0];

                highestNum = array[i];
                highestNumsIndex[0] = i; // gets the index of the current highest number
            }
            else if(array[i] > secondHighestNum)
            {
                //push third number to the left
                thirdHighestNum = secondHighestNum;
                highestNumsIndex[2] = highestNumsIndex[1];

                secondHighestNum = array[i];//Set val
                highestNumsIndex[1] = i;//Set index
            }

            else if(array[i] > thirdHighestNum)
            {
                thirdHighestNum = array[i];//Set val
                highestNumsIndex[2] = i;// Set index
            }
        }


        return highestNumsIndex;
    }

    public static float Map(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }
    /// <summary>
    /// Sorts to game-objects by name
    /// </summary>
    /// <param name="o1">GameObject 1</param>
    /// <param name="o2">GameObject 2</param>
    /// <returns>Returns an integer that indicates the relative position of the two GameObjects in the list </returns>
    public static int SortByName(GameObject o1, GameObject o2) {
        // https://docs.microsoft.com/en-us/dotnet/api/System.String.Compare?view=net-6.0
        return String.Compare(o1.name, o2.name, StringComparison.Ordinal);
    }

    /*
     *  index = -1
     *         int max = int.MinValue;
        for (int i = 0; i < array.Length; ++i)
        {
            if (array[i] > max)
            {
                max = array[i];
                index = i;
            }
        }
        */
}
