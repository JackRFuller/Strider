using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WoundChart
{                                                       //Defense x Strength
                                                        //0 1 2 3 4 5 6 7 8 9
    private static int[,] woundChart = new int[10,10] { { 4,5,5,6,6,10,11,12,100,100 },
                                                        { 4,4,5,5,6,6,10,11,12,100 },
                                                        { 3,4,4,5,5,6,6,10,11,12 },
                                                        { 3,3,4,4,5,5,6,6,10,11 },
                                                        { 3,3,3,4,4,5,5,6,6,10 },
                                                        { 3,3,3,3,4,4,5,5,6,6 },
                                                        { 3,3,3,3,3,4,4,5,5,6 },
                                                        { 3,3,3,3,3,3,4,4,5,5 },
                                                        { 3,3,3,3,3,3,3,4,4,5 },
                                                        { 3,3,3,3,3,3,3,3,4,4 }};

    public static int ValueNeededToCauseAWound(int defense,int strength)
    {
        return woundChart[defense -1, strength-1];
    }
}
