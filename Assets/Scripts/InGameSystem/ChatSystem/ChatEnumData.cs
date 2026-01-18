using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이후 추가 및 변경
public enum ChatSpeakerType //0~99 : 주역, 100~199 : 조역, 200~ : 엑스트라, 999=???
{
    None = 0,
    Icarus = 1,
    Karok = 2,
    Eve = 3,
    Hani = 4,
    Person5 = 5,

    IcarusDad = 101,
    Kar = 102,

    ExtraRodot1 =201,
    ExtraRodot2 = 202,
    ExtraPolice = 203,
    QuestionablePerson1 = 204,
    QuestionablePerson2 = 205,
    Bartender = 206,
    SercurityRobot = 207,

    Extra = 999,
}

public enum BackgroundImage//대화 배경 이미지
{
    None = 0,
    Black = 1,
    Eden = 2,
    Etc = 3,
}

public enum ChatImage //시네마틱용 이미지
{
    nothing,
    openlog1,
    openlog2,
    openlog3,
    openlog4,
    openlog5,
    openlog6,
    openlog7,
    openlog8,
}
