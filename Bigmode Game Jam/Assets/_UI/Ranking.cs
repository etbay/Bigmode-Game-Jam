using System;
using UnityEngine;
public class Ranking : MonoBehaviour
{
    [System.Serializable]
    public struct RankRequirements
    {
        // each double represents the time in seconds that is required for the rank
        public double dReq;
        public double cReq;
        public double bReq;
        public double aReq;
        public double sReq;
    }
    public enum Rank
    {
        DRank = 0,
        CRank = 1,
        BRank = 2,
        ARank = 3,
        SRank = 4
    }
    public static Rank GenerateRank(RankRequirements level, double playerTime)
    {
        if (playerTime < level.sReq)
        {
            return Rank.SRank;
        }
        else if (playerTime < level.aReq)
        {
            return Rank.ARank;
        }
        else if (playerTime < level.bReq)
        {
            return Rank.BRank;
        }
        else if (playerTime < level.cReq)
        {
            return Rank.CRank;
        }
        else
        {
            return Rank.DRank;
        }
    }
}
