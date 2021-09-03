using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PluginScript : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TwoStrings {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        string string1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        string string2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        string concatenated;

        //Constructor to inicialize the data
        public TwoStrings(string s1, string s2, string c = " ") {
            string1 = s1;
            string2 = s2;
            concatenated = c;
        }

        //Function to print the data 
        public override string ToString()
        {
            return "Structure:  - \nString1: " + string1 + "\t String2: " + string2 + "\t Concatenated: " + concatenated;
        }
    }

    //Function to comunicate the data with C#, the struct TwoStrings is sent that by using the modifiers
	//that determinte if the parameter is modified [ref (may) in (cannot) out (must)]
    [DllImport("Plugin", CallingConvention = CallingConvention.StdCall)]
    public static extern void concatenateStrings([In][Out] ref TwoStrings ts);


    // Start is called before the first frame update
    public void Start()
    {


        TwoStrings ts = new TwoStrings ("Simena", "Dinas");

        
        concatenateStrings(ref ts);

        //print the modified structure, which concatenate string1 and string2 and save the result in concatenated
        Debug.Log("TS string :: " + ts);
       

    }

}
