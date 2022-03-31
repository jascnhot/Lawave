using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public enum goodAtInfoEnum
    {
        //律師Type列表
        未選擇 = -1,
        民事訴訟 = 0,
        刑事訴訟 = 1,
        家事訴訟 = 2,
        勞資爭議 = 3,
        消費糾紛 = 4,
        其他 = 5
    }
    //case Type列表
    public enum caseTypeEnum
    {
        未選擇 = -1,
        婚姻諮詢 = 0,
        土地糾紛 = 1,
        車禍調解 = 2,
        金錢糾紛 = 3,
        勞資糾紛 = 4,
        其他事由 = 5
    }

    public enum areaEnum
    {
        //律師地區列表
        未選擇 = -1,
        台北 = 0,
        新北 = 1,
        桃園 = 2,
        台中 = 3,
        台南 = 4,
        高雄 = 5,
        新竹 = 6,
        苗栗 = 7,
        彰化 = 8,
        南投 = 9,
        雲林 = 10,
        嘉義 = 11,
        屏東 = 12,
        宜蘭 = 13,
        花蓮 = 14,
        台東 = 15,
        澎湖 = 16,
        金門 = 17,
        連江 = 18,
        其他 = 19
    }

    public enum appointmentStatus
    {
        canuse = 0,
        audit = 1,
        booked = 2,
        completed = 3,
        reject = 4
        
    }
}