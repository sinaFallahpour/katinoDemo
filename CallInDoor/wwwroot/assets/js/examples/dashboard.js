'use strict';
var dataSeries = [
    [{
        "date": "1398-01-01",
        "value": 20000000
    },
    {
        "date": "1398-01-02",
        "value": 10379978
    },
    {
        "date": "1398-01-03",
        "value": 30493749
    },
    {
        "date": "1398-01-04",
        "value": 10785250
    },
    {
        "date": "1398-01-05",
        "value": 33901904
    },
    {
        "date": "1398-01-06",
        "value": 11576838
    },
    {
        "date": "1398-01-07",
        "value": 14413854
    },
    {
        "date": "1398-01-08",
        "value": 15177211
    },
    {
        "date": "1398-01-09",
        "value": 16622100
    },
    {
        "date": "1398-01-10",
        "value": 17381072
    },
    {
        "date": "1398-01-11",
        "value": 18802310
    },
    {
        "date": "1398-01-12",
        "value": 15531790
    },
    {
        "date": "1398-01-13",
        "value": 15748881
    },
    {
        "date": "1398-01-14",
        "value": 18706437
    },
    {
        "date": "1398-01-15",
        "value": 19752685
    },
    {
        "date": "1398-01-16",
        "value": 21016418
    },
    {
        "date": "1398-01-17",
        "value": 25622924
    },
    {
        "date": "1398-01-18",
        "value": 25337480
    },
    {
        "date": "1398-01-19",
        "value": 22258882
    },
    {
        "date": "1398-01-20",
        "value": 23829538
    },
    {
        "date": "1398-01-21",
        "value": 24245689
    },
    {
        "date": "1398-01-22",
        "value": 26429711
    },
    {
        "date": "1398-01-23",
        "value": 26259017
    },
    {
        "date": "1398-01-24",
        "value": 25396183
    },
    {
        "date": "1398-01-25",
        "value": 23107346
    },
    {
        "date": "1398-01-26",
        "value": 28659852
    },
    {
        "date": "1398-01-27",
        "value": 25270783
    },
    {
        "date": "1398-01-28",
        "value": 26270783
    },
    {
        "date": "1398-01-29",
        "value": 27270783
    },
    {
        "date": "1398-01-30",
        "value": 28270783
    },
    {
        "date": "1398-01-31",
        "value": 29270783
    },
    {
        "date": "1398-02-01",
        "value": 30270783
    },
    {
        "date": "1398-02-02",
        "value": 31270783
    },
    {
        "date": "1398-02-03",
        "value": 32270783
    },
    {
        "date": "1398-02-04",
        "value": 33270783
    },
    {
        "date": "1398-02-05",
        "value": 28270783
    },
    {
        "date": "1398-02-06",
        "value": 27270783
    },
    {
        "date": "1398-02-07",
        "value": 35270783
    },
    {
        "date": "1398-02-08",
        "value": 34270783
    },
    {
        "date": "1398-02-09",
        "value": 28270783
    },
    {
        "date": "1398-02-10",
        "value": 35270783
    },
    {
        "date": "1398-02-11",
        "value": 36270783
    },
    {
        "date": "1398-02-12",
        "value": 34127078
    },
    {
        "date": "1398-02-13",
        "value": 33124078
    },
    {
        "date": "1398-02-14",
        "value": 36227078
    },
    {
        "date": "1398-02-15",
        "value": 37827078
    },
    {
        "date": "1398-02-16",
        "value": 36427073
    },
    {
        "date": "1398-02-17",
        "value": 37570783
    },
    {
        "date": "1398-02-18",
        "value": 38627073
    },
    {
        "date": "1398-02-19",
        "value": 37727078
    },
    {
        "date": "1398-02-20",
        "value": 38827073
    },
    {
        "date": "1398-02-21",
        "value": 40927078
    },
    {
        "date": "1398-02-22",
        "value": 41027078
    },
    {
        "date": "1398-02-23",
        "value": 42127073
    },
    {
        "date": "1398-02-24",
        "value": 43220783
    },
    {
        "date": "1398-02-25",
        "value": 44327078
    },
    {
        "date": "1398-02-26",
        "value": 40427078
    },
    {
        "date": "1398-02-27",
        "value": 41027078
    },
    {
        "date": "1398-02-28",
        "value": 45627078
    },
    {
        "date": "1398-03-01",
        "value": 44727078
    },
    {
        "date": "1398-03-02",
        "value": 44227078
    },
    {
        "date": "1398-03-03",
        "value": 45227078
    },
    {
        "date": "1398-03-04",
        "value": 46027078
    },
    {
        "date": "1398-03-05",
        "value": 46927078
    },
    {
        "date": "1398-03-06",
        "value": 47027078
    },
    {
        "date": "1398-03-07",
        "value": 46227078
    },
    {
        "date": "1398-03-08",
        "value": 47027078
    },
    {
        "date": "1398-03-09",
        "value": 48027078
    },
    {
        "date": "1398-03-10",
        "value": 47027078
    },
    {
        "date": "1398-03-11",
        "value": 47027078
    },
    {
        "date": "1398-03-12",
        "value": 48017078
    },
    {
        "date": "1398-03-13",
        "value": 48077078
    },
    {
        "date": "1398-03-14",
        "value": 48087078
    },
    {
        "date": "1398-03-15",
        "value": 48017078
    },
    {
        "date": "1398-03-16",
        "value": 48047078
    },
    {
        "date": "1398-03-17",
        "value": 48067078
    },
    {
        "date": "1398-03-18",
        "value": 48077078
    },
    {
        "date": "1398-03-19",
        "value": 48027074
    },
    {
        "date": "1398-03-20",
        "value": 48927079
    },
    {
        "date": "1398-03-21",
        "value": 48727071
    },
    {
        "date": "1398-03-22",
        "value": 48127072
    },
    {
        "date": "1398-03-23",
        "value": 48527072
    },
    {
        "date": "1398-03-24",
        "value": 48627027
    },
    {
        "date": "1398-03-25",
        "value": 48027040
    },
    {
        "date": "1398-03-26",
        "value": 48027043
    },
    {
        "date": "1398-03-27",
        "value": 48057022
    },
    {
        "date": "1398-03-28",
        "value": 49057022
    },
    {
        "date": "1398-03-29",
        "value": 50057022
    },
    {
        "date": "1398-03-30",
        "value": 51057022
    },
    {
        "date": "1398-03-31",
        "value": 52057022
    },
    {
        "date": "1398-04-01",
        "value": 53057022
    },
    {
        "date": "1398-04-02",
        "value": 54057022
    },
    {
        "date": "1398-04-03",
        "value": 52057022
    },
    {
        "date": "1398-04-04",
        "value": 55057022
    },
    {
        "date": "1398-04-05",
        "value": 58270783
    },
    {
        "date": "1398-04-06",
        "value": 56270783
    },
    {
        "date": "1398-04-07",
        "value": 55270783
    },
    {
        "date": "1398-04-08",
        "value": 58270783
    },
    {
        "date": "1398-04-09",
        "value": 59270783
    },
    {
        "date": "1398-04-10",
        "value": 60270783
    },
    {
        "date": "1398-04-11",
        "value": 61270783
    },
    {
        "date": "1398-04-12",
        "value": 62270783
    },
    {
        "date": "1398-04-13",
        "value": 63270783
    },
    {
        "date": "1398-04-14",
        "value": 64270783
    },
    {
        "date": "1398-04-15",
        "value": 65270783
    },
    {
        "date": "1398-04-16",
        "value": 66270783
    },
    {
        "date": "1398-04-17",
        "value": 67270783
    },
    {
        "date": "1398-04-18",
        "value": 68270783
    },
    {
        "date": "1398-04-19",
        "value": 69270783
    },
    {
        "date": "1398-04-20",
        "value": 70270783
    },
    {
        "date": "1398-04-21",
        "value": 71270783
    },
    {
        "date": "1398-04-22",
        "value": 72270783
    },
    {
        "date": "1398-04-23",
        "value": 73270783
    },
    {
        "date": "1398-04-24",
        "value": 74270783
    },
    {
        "date": "1398-04-25",
        "value": 75270783
    },
    {
        "date": "1398-04-26",
        "value": 76660783
    },
    {
        "date": "1398-04-27",
        "value": 77270783
    },
    {
        "date": "1398-04-28",
        "value": 78370783
    },
    {
        "date": "1398-04-29",
        "value": 79470783
    },
    {
        "date": "1398-04-30",
        "value": 80170783
    }
    ],
    [{
        "date": "1398-01-01",
        "value": 150000000
    },
    {
        "date": "1398-01-02",
        "value": 160379978
    },
    {
        "date": "1398-01-03",
        "value": 170493749
    },
    {
        "date": "1398-01-04",
        "value": 160785250
    },
    {
        "date": "1398-01-05",
        "value": 167391904
    },
    {
        "date": "1398-01-06",
        "value": 161576838
    },
    {
        "date": "1398-01-07",
        "value": 161413854
    },
    {
        "date": "1398-01-08",
        "value": 152177211
    },
    {
        "date": "1398-01-09",
        "value": 140762210
    },
    {
        "date": "1398-01-10",
        "value": 144381072
    },
    {
        "date": "1398-01-11",
        "value": 154352310
    },
    {
        "date": "1398-01-12",
        "value": 165531790
    },
    {
        "date": "1398-01-13",
        "value": 175748881
    },
    {
        "date": "1398-01-14",
        "value": 187064037
    },
    {
        "date": "1398-01-15",
        "value": 197520685
    },
    {
        "date": "1398-01-16",
        "value": 210176418
    },
    {
        "date": "1398-01-17",
        "value": 196122924
    },
    {
        "date": "1398-01-18",
        "value": 207337480
    },
    {
        "date": "1398-01-19",
        "value": 200258882
    },
    {
        "date": "1398-01-20",
        "value": 186829538
    },
    {
        "date": "1398-01-21",
        "value": 192456897
    },
    {
        "date": "1398-01-22",
        "value": 204299711
    },
    {
        "date": "1398-01-23",
        "value": 192759017
    },
    {
        "date": "1398-01-24",
        "value": 203596183
    },
    {
        "date": "1398-01-25",
        "value": 208107346
    },
    {
        "date": "1398-01-26",
        "value": 196359852
    },
    {
        "date": "1398-01-27",
        "value": 192570783
    },
    {
        "date": "1398-01-28",
        "value": 177967768
    },
    {
        "date": "1398-01-29",
        "value": 190632803
    },
    {
        "date": "1398-01-30",
        "value": 203725316
    },
    {
        "date": "1398-01-31",
        "value": 218226177
    },
    {
        "date": "1398-02-01",
        "value": 210698669
    },
    {
        "date": "1398-02-02",
        "value": 217640656
    },
    {
        "date": "1398-02-03",
        "value": 216142362
    },
    {
        "date": "1398-02-04",
        "value": 139810971
    },
    {
        "date": "1398-02-05",
        "value": 196704289
    },
    {
        "date": "1398-02-06",
        "value": 190436945
    },
    {
        "date": "1398-02-07",
        "value": 178891686
    },
    {
        "date": "1398-02-08",
        "value": 171613962
    },
    {
        "date": "1398-02-09",
        "value": 157579773
    },
    {
        "date": "1398-02-10",
        "value": 158677098
    },
    {
        "date": "1398-02-11",
        "value": 147129977
    },
    {
        "date": "1398-02-12",
        "value": 151561876
    },
    {
        "date": "1398-02-13",
        "value": 151627421
    },
    {
        "date": "1398-02-14",
        "value": 143543872
    },
    {
        "date": "1398-02-15",
        "value": 136581057
    },
    {
        "date": "1398-02-16",
        "value": 135560715
    },
    {
        "date": "1398-02-17",
        "value": 122625263
    },
    {
        "date": "1398-02-18",
        "value": 112091484
    },
    {
        "date": "1398-02-19",
        "value": 98810329
    },
    {
        "date": "1398-02-20",
        "value": 99882912
    },
    {
        "date": "1398-02-21",
        "value": 94943095
    },
    {
        "date": "1398-02-22",
        "value": 104875743
    },
    {
        "date": "1398-02-23",
        "value": 116383678
    },
    {
        "date": "1398-02-24",
        "value": 125028841
    },
    {
        "date": "1398-02-25",
        "value": 123967310
    },
    {
        "date": "1398-02-26",
        "value": 133167029
    },
    {
        "date": "1398-02-27",
        "value": 128577263
    },
    {
        "date": "1398-02-28",
        "value": 115836969
    },
    {
        "date": "1398-03-01",
        "value": 119264529
    },
    {
        "date": "1398-03-02",
        "value": 109363374
    },
    {
        "date": "1398-03-03",
        "value": 113985628
    },
    {
        "date": "1398-03-04",
        "value": 114650999
    },
    {
        "date": "1398-03-05",
        "value": 110866108
    },
    {
        "date": "1398-03-06",
        "value": 96473454
    },
    {
        "date": "1398-03-07",
        "value": 104075886
    },
    {
        "date": "1398-03-08",
        "value": 103568384
    },
    {
        "date": "1398-03-09",
        "value": 101534883
    },
    {
        "date": "1398-03-10",
        "value": 115825447
    },
    {
        "date": "1398-03-11",
        "value": 126133916
    },
    {
        "date": "1398-03-12",
        "value": 116502109
    },
    {
        "date": "1398-03-13",
        "value": 130169411
    },
    {
        "date": "1398-03-14",
        "value": 124296886
    },
    {
        "date": "1398-03-15",
        "value": 126347399
    },
    {
        "date": "1398-03-16",
        "value": 131483669
    },
    {
        "date": "1398-03-17",
        "value": 142811333
    },
    {
        "date": "1398-03-18",
        "value": 129675396
    },
    {
        "date": "1398-03-19",
        "value": 115514483
    },
    {
        "date": "1398-03-20",
        "value": 117630630
    },
    {
        "date": "1398-03-21",
        "value": 122340239
    },
    {
        "date": "1398-03-22",
        "value": 132349091
    },
    {
        "date": "1398-03-23",
        "value": 125613305
    },
    {
        "date": "1398-03-24",
        "value": 135592466
    },
    {
        "date": "1398-03-25",
        "value": 123408762
    },
    {
        "date": "1398-03-26",
        "value": 111991454
    },
    {
        "date": "1398-03-27",
        "value": 116123955
    },
    {
        "date": "1398-03-28",
        "value": 112817214
    },
    {
        "date": "1398-03-29",
        "value": 113029590
    },
    {
        "date": "1398-03-30",
        "value": 108753398
    },
    {
        "date": "1398-03-31",
        "value": 99383763
    },
    {
        "date": "1398-04-01",
        "value": 100151737
    },
    {
        "date": "1398-04-02",
        "value": 94985209
    },
    {
        "date": "1398-04-03",
        "value": 82913669
    },
    {
        "date": "1398-04-04",
        "value": 78748268
    },
    {
        "date": "1398-04-05",
        "value": 63829135
    },
    {
        "date": "1398-04-06",
        "value": 78694727
    },
    {
        "date": "1398-04-07",
        "value": 80868994
    },
    {
        "date": "1398-04-08",
        "value": 93799013
    },
    {
        "date": "1398-04-09",
        "value": 99042416
    },
    {
        "date": "1398-04-10",
        "value": 97298692
    },
    {
        "date": "1398-04-11",
        "value": 83353499
    },
    {
        "date": "1398-04-12",
        "value": 71248129
    },
    {
        "date": "1398-04-13",
        "value": 75253744
    },
    {
        "date": "1398-04-14",
        "value": 68976648
    },
    {
        "date": "1398-04-15",
        "value": 71002284
    },
    {
        "date": "1398-04-16",
        "value": 75052401
    },
    {
        "date": "1398-04-17",
        "value": 83894030
    },
    {
        "date": "1398-04-18",
        "value": 90236528
    },
    {
        "date": "1398-04-19",
        "value": 99739114
    },
    {
        "date": "1398-04-20",
        "value": 96407136
    },
    {
        "date": "1398-04-21",
        "value": 108323177
    },
    {
        "date": "1398-04-22",
        "value": 101578914
    },
    {
        "date": "1398-04-23",
        "value": 115877608
    },
    {
        "date": "1398-04-24",
        "value": 112088857
    },
    {
        "date": "1398-04-25",
        "value": 112071353
    },
    {
        "date": "1398-04-26",
        "value": 101790062
    },
    {
        "date": "1398-04-27",
        "value": 115003761
    },
    {
        "date": "1398-04-28",
        "value": 120457727
    },
    {
        "date": "1398-04-29",
        "value": 118253926
    },
    {
        "date": "1398-04-30",
        "value": 117956992
    }
    ],
    [{
        "date": "1398-01-01",
        "value": 50000000
    },
    {
        "date": "1398-01-02",
        "value": 60379978
    },
    {
        "date": "1398-01-03",
        "value": 40493749
    },
    {
        "date": "1398-01-04",
        "value": 60785250
    },
    {
        "date": "1398-01-05",
        "value": 67391904
    },
    {
        "date": "1398-01-06",
        "value": 61576838
    },
    {
        "date": "1398-01-07",
        "value": 61413854
    },
    {
        "date": "1398-01-08",
        "value": 82177211
    },
    {
        "date": "1398-01-09",
        "value": 103762210
    },
    {
        "date": "1398-01-10",
        "value": 84381072
    },
    {
        "date": "1398-01-11",
        "value": 54352310
    },
    {
        "date": "1398-01-12",
        "value": 65531790
    },
    {
        "date": "1398-01-13",
        "value": 75748881
    },
    {
        "date": "1398-01-14",
        "value": 47064037
    },
    {
        "date": "1398-01-15",
        "value": 67520685
    },
    {
        "date": "1398-01-16",
        "value": 60176418
    },
    {
        "date": "1398-01-17",
        "value": 66122924
    },
    {
        "date": "1398-01-18",
        "value": 57337480
    },
    {
        "date": "1398-01-19",
        "value": 100258882
    },
    {
        "date": "1398-01-20",
        "value": 46829538
    },
    {
        "date": "1398-01-21",
        "value": 92456897
    },
    {
        "date": "1398-01-22",
        "value": 94299711
    },
    {
        "date": "1398-01-23",
        "value": 62759017
    },
    {
        "date": "1398-01-24",
        "value": 103596183
    },
    {
        "date": "1398-01-25",
        "value": 108107346
    },
    {
        "date": "1398-01-26",
        "value": 66359852
    },
    {
        "date": "1398-01-27",
        "value": 62570783
    },
    {
        "date": "1398-01-28",
        "value": 77967768
    },
    {
        "date": "1398-01-29",
        "value": 60632803
    },
    {
        "date": "1398-01-30",
        "value": 103725316
    },
    {
        "date": "1398-01-31",
        "value": 98226177
    },
    {
        "date": "1398-02-01",
        "value": 60698669
    },
    {
        "date": "1398-02-02",
        "value": 67640656
    },
    {
        "date": "1398-02-03",
        "value": 66142362
    },
    {
        "date": "1398-02-04",
        "value": 101410971
    },
    {
        "date": "1398-02-05",
        "value": 66704289
    },
    {
        "date": "1398-02-06",
        "value": 60436945
    },
    {
        "date": "1398-02-07",
        "value": 78891686
    },
    {
        "date": "1398-02-08",
        "value": 71613962
    },
    {
        "date": "1398-02-09",
        "value": 107579773
    },
    {
        "date": "1398-02-10",
        "value": 58677098
    },
    {
        "date": "1398-02-11",
        "value": 87129977
    },
    {
        "date": "1398-02-12",
        "value": 51561876
    },
    {
        "date": "1398-02-13",
        "value": 51627421
    },
    {
        "date": "1398-02-14",
        "value": 83543872
    },
    {
        "date": "1398-02-15",
        "value": 66581057
    },
    {
        "date": "1398-02-16",
        "value": 65560715
    },
    {
        "date": "1398-02-17",
        "value": 62625263
    },
    {
        "date": "1398-02-18",
        "value": 92091484
    },
    {
        "date": "1398-02-19",
        "value": 48810329
    },
    {
        "date": "1398-02-20",
        "value": 49882912
    },
    {
        "date": "1398-02-21",
        "value": 44943095
    },
    {
        "date": "1398-02-22",
        "value": 104875743
    },
    {
        "date": "1398-02-23",
        "value": 96383678
    },
    {
        "date": "1398-02-24",
        "value": 105028841
    },
    {
        "date": "1398-02-25",
        "value": 63967310
    },
    {
        "date": "1398-02-26",
        "value": 63167029
    },
    {
        "date": "1398-02-27",
        "value": 68577263
    },
    {
        "date": "1398-02-28",
        "value": 95836969
    },
    {
        "date": "1398-03-01",
        "value": 99264529
    },
    {
        "date": "1398-03-02",
        "value": 109363374
    },
    {
        "date": "1398-03-03",
        "value": 93985628
    },
    {
        "date": "1398-03-04",
        "value": 94650999
    },
    {
        "date": "1398-03-05",
        "value": 90866108
    },
    {
        "date": "1398-03-06",
        "value": 46473454
    },
    {
        "date": "1398-03-07",
        "value": 84075886
    },
    {
        "date": "1398-03-08",
        "value": 103568384
    },
    {
        "date": "1398-03-09",
        "value": 101534883
    },
    {
        "date": "1398-03-10",
        "value": 95825447
    },
    {
        "date": "1398-03-11",
        "value": 66133916
    },
    {
        "date": "1398-03-12",
        "value": 96502109
    },
    {
        "date": "1398-03-13",
        "value": 80169411
    },
    {
        "date": "1398-03-14",
        "value": 84296886
    },
    {
        "date": "1398-03-15",
        "value": 86347399
    },
    {
        "date": "1398-03-16",
        "value": 31483669
    },
    {
        "date": "1398-03-17",
        "value": 82811333
    },
    {
        "date": "1398-03-18",
        "value": 89675396
    },
    {
        "date": "1398-03-19",
        "value": 95514483
    },
    {
        "date": "1398-03-20",
        "value": 97630630
    },
    {
        "date": "1398-03-21",
        "value": 62340239
    },
    {
        "date": "1398-03-22",
        "value": 62349091
    },
    {
        "date": "1398-03-23",
        "value": 65613305
    },
    {
        "date": "1398-03-24",
        "value": 65592466
    },
    {
        "date": "1398-03-25",
        "value": 63408762
    },
    {
        "date": "1398-03-26",
        "value": 91991454
    },
    {
        "date": "1398-03-27",
        "value": 96123955
    },
    {
        "date": "1398-03-28",
        "value": 92817214
    },
    {
        "date": "1398-03-29",
        "value": 93029590
    },
    {
        "date": "1398-03-30",
        "value": 108753398
    },
    {
        "date": "1398-03-31",
        "value": 49383763
    },
    {
        "date": "1398-04-01",
        "value": 100151737
    },
    {
        "date": "1398-04-02",
        "value": 44985209
    },
    {
        "date": "1398-04-03",
        "value": 52913669
    },
    {
        "date": "1398-04-04",
        "value": 48748268
    },
    {
        "date": "1398-04-05",
        "value": 23829135
    },
    {
        "date": "1398-04-06",
        "value": 58694727
    },
    {
        "date": "1398-04-07",
        "value": 50868994
    },
    {
        "date": "1398-04-08",
        "value": 43799013
    },
    {
        "date": "1398-04-09",
        "value": 4042416
    },
    {
        "date": "1398-04-10",
        "value": 47298692
    },
    {
        "date": "1398-04-11",
        "value": 53353499
    },
    {
        "date": "1398-04-12",
        "value": 71248129
    },
    {
        "date": "1398-04-13",
        "value": 75253744
    },
    {
        "date": "1398-04-14",
        "value": 68976648
    },
    {
        "date": "1398-04-15",
        "value": 71002284
    },
    {
        "date": "1398-04-16",
        "value": 75052401
    },
    {
        "date": "1398-04-17",
        "value": 83894030
    },
    {
        "date": "1398-04-18",
        "value": 50236528
    },
    {
        "date": "1398-04-19",
        "value": 59739114
    },
    {
        "date": "1398-04-20",
        "value": 56407136
    },
    {
        "date": "1398-04-21",
        "value": 108323177
    },
    {
        "date": "1398-04-22",
        "value": 101578914
    },
    {
        "date": "1398-04-23",
        "value": 95877608
    },
    {
        "date": "1398-04-24",
        "value": 62088857
    },
    {
        "date": "1398-04-25",
        "value": 92071353
    },
    {
        "date": "1398-04-26",
        "value": 81790062
    },
    {
        "date": "1398-04-27",
        "value": 105003761
    },
    {
        "date": "1398-04-28",
        "value": 100457727
    },
    {
        "date": "1398-04-29",
        "value": 98253926
    },
    {
        "date": "1398-04-30",
        "value": 67956992
    }
    ]
]
$(document).ready(function () {
    // Apex.chart = {
    //     fontFamily: 'inherit',
    //     locales: [{
    //         "name": "fa",
    //         "options": {
    //             "months": ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهرویور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
    //             "shortMonths": ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهرویور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
    //             "days": ["یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"],
    //             "shortDays": ["ی", "د", "س", "چ", "پ", "ج", "ش"],
    //             "toolbar": {
    //                 "exportToSVG": "دریافت SVG",
    //                 "exportToPNG": "دریافت PNG",
    //                 "menu": "فهرست",
    //                 "selection": "انتخاب",
    //                 "selectionZoom": "بزرگنمایی قسمت انتخاب شده",
    //                 "zoomIn": "بزرگ نمایی",
    //                 "zoomOut": "کوچک نمایی",
    //                 "pan": "جا به جایی",
    //                 "reset": "بازنشانی بزرگ نمایی"
    //             }
    //         }
    //     }],
    //     defaultLocale: "fa"
    // }
    var colors = {
        primary: $('.colors .bg-primary').css('background-color'),
        primaryLight: $('.colors .bg-primary-bright').css('background-color'),
        secondary: $('.colors .bg-secondary').css('background-color'),
        secondaryLight: $('.colors .bg-secondary-bright').css('background-color'),
        info: $('.colors .bg-info').css('background-color'),
        infoLight: $('.colors .bg-info-bright').css('background-color'),
        success: $('.colors .bg-success').css('background-color'),
        successLight: $('.colors .bg-success-bright').css('background-color'),
        danger: $('.colors .bg-danger').css('background-color'),
        dangerLight: $('.colors .bg-danger-bright').css('background-color'),
        warning: $('.colors .bg-warning').css('background-color'),
        warningLight: $('.colors .bg-warning-bright').css('background-color'),
    };

    /**
     *  Slick slide example
     **/

    if ($('.slick-single-item').length) {
        $('.slick-single-item').slick({
            autoplay: true,
            autoplaySpeed: 3000,
            infinite: true,
            slidesToShow: 4,
            slidesToScroll: 4,
            prevArrow: '.slick-single-arrows a:eq(0)',
            nextArrow: '.slick-single-arrows a:eq(1)',
            responsive: [
                {
                    breakpoint: 1300,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                    }
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                    }
                },
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2
                    }
                },
                {
                    breakpoint: 540,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
    }

    if ($('.reportrange').length > 0) {


        var start = moment().subtract(29, 'days');
        var end = moment();

        function cb(start, end) {

            var start = moment(start); // pass your date obj here.
            console.log(start.format('jYYYY/jM/jD'));

            var end = moment(end); // pass your date obj here.
            console.log(end.format('jYYYY/jM/jD'));


            $('.reportrange .text').html(start.format('jYYYY/jM/jD') + ' - ' + end.format('jYYYY/jM/jD'));
        }

        $('.reportrange').daterangepicker({

            months: ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'],

            startDate: start,
            endDate: end,
            ranges: {
                'امروز': [moment(), moment()],
                'دیروز': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'هفته گذشته': [moment().subtract(6, 'days'), moment()],
                'ماه گذشته': [moment().subtract(29, 'days'), moment()],
                'این ماه': [moment().startOf('month'), moment().endOf('month')],
                'آخرین ماه': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            "locale": {
                "format": "jYYYY/jM/jD",
                "separator": " - ",
                "applyLabel": "اعمال",
                "cancelLabel": "انصراف",
                "fromLabel": "از",
                "toLabel": "تا",
                "customRangeLabel": "سفارشی",
                "weekLabel": "هف",
                "daysOfWeek": [
                    "ی",
                    "د",
                    "س",
                    "چ",
                    "پ",
                    "ج",
                    "ش"
                ],
                "monthNames": [
                    "ژانویه",
                    "فوریه",
                    "مارس",
                    "آوریل",
                    "می",
                    "ژوئن",
                    "جولای",
                    "آگوست",
                    "سپتامبر",
                    "اکتبر",
                    "نوامبر",
                    "دسامبر"
                ],
                "firstDay": 6
            }

        }, cb);

        cb(start, end);


        // console.log("start.format('jYYYY/jM/jD')",start.format('jYYYY/jM/jD') );
        // console.log("end.format('jYYYY/jM/jD')",end.format('jYYYY/jM/jD') );
        //console.log("cb.format('jYYYY/jM/jD')",cb.format('jYYYY/jM/jD') );
    }

    var chartColors = {
        primary: {
            base: '#3f51b5',
            light: '#c0c5e4'
        },
        danger: {
            base: '#f2125e',
            light: '#fcd0df'
        },
        success: {
            base: '#0acf97',
            light: '#cef5ea'
        },
        warning: {
            base: '#ff8300',
            light: '#ffe6cc'
        },
        info: {
            base: '#00bcd4',
            light: '#e1efff'
        },
        dark: '#37474f',
        facebook: '#3b5998',
        twitter: '#55acee',
        linkedin: '#0077b5',
        instagram: '#517fa4',
        whatsapp: '#25D366',
        dribbble: '#ea4c89',
        google: '#DB4437',
        borderColor: '#e8e8e8',
        fontColor: '#999'
    };

    if ($('body').hasClass('dark')) {
        chartColors.borderColor = 'rgba(255, 255, 255, .1)';
        chartColors.fontColor = 'rgba(255, 255, 255, .4)';
    }

    /// Chartssssss

    chart_demo_1();

    chart_demo_2();

    chart_demo_3();

    chart_demo_4();

    chart_demo_5();

    chart_demo_6();

    chart_demo_7();

    chart_demo_8();

    chart_demo_9();

    chart_demo_10();

    function chart_demo_1() {
        if ($('#chart_demo_1').length) {
            var element = document.getElementById("chart_demo_1");
            element.height = 146;
            new Chart(element, {
                type: 'bar',
                data: {
                    labels: ["1390", "1391", "1392", "1393", "1394", "1395", "1396", "1397"],
                    datasets: [
                        {
                            label: "Total Sales",
                            backgroundColor: colors.primary,
                            data: [133, 221, 783, 978, 214, 421, 211, 577]
                        }, {
                            label: "Average",
                            backgroundColor: colors.info,
                            data: [408, 947, 675, 734, 325, 672, 632, 213]
                        }
                    ]
                },
                options: {
                    legend: {
                        display: false
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                fontSize: 11,
                                fontColor: chartColors.fontColor
                            },
                            gridLines: {
                                display: false,
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                fontSize: 11,
                                fontColor: chartColors.fontColor
                            },
                            gridLines: {
                                color: chartColors.borderColor
                            }
                        }],
                    }
                }
            })
        }
    }

    function chart_demo_2() {
        if ($('#chart_demo_2').length) {
            var ctx = document.getElementById('chart_demo_2').getContext('2d');
            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["مهر 1398", "آبان 1398", "دی 1398", "بهمن 1398", "اسفند 1398", "فروردین 1398", "اردیبعشت 1398", "اردیبعشت 1399", "اسفند 1399", "بهمن 1399", "دی 1399", "آذر 1399"],
                    datasets: [{
                        label: "Rainfall",
                        backgroundColor: chartColors.primary.light,
                        borderColor: chartColors.primary.base,
                        data: [26.4, 39.8, 66.8, 66.4, 40.6, 55.2, 77.4, 69.8, 57.8, 76, 110.8, 142.6],
                    }]
                },
                options: {
                    legend: {
                        display: false,
                        labels: {
                            fontColor: chartColors.fontColor
                        }
                    },
                    title: {
                        display: true,
                        text: 'پیشبینی در توکیو',
                        fontColor: chartColors.fontColor,
                    },
                    scales: {
                        yAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor,
                                beginAtZero: true
                            },
                            scaleLabel: {
                                display: true,
                                labelString: 'پیشبینی در ..',
                                fontColor: chartColors.fontColor,
                            }
                        }],
                        xAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
    }

    function chart_demo_3() {
        if ($('#chart_demo_3').length) {
            var element = document.getElementById("chart_demo_3"),
                ctx = element.getContext("2d");


            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
                    datasets: [{
                        label: 'موفقیت',
                        borderColor: colors.success,
                        data: [-10, 30, -20, 0, 25, 44, 30, 15, 20, 10, 5, -5],
                        pointRadius: 5,
                        pointHoverRadius: 7,
                        borderDash: [2, 2],
                        fill: false
                    }, {
                        label: 'برگشتی',
                        fill: false,
                        borderDash: [2, 2],
                        borderColor: colors.danger,
                        data: [20, 0, 22, 39, -10, 19, -7, 0, 15, 0, -10, 5],
                        pointRadius: 5,
                        pointHoverRadius: 7
                    }]
                },
                options: {
                    responsive: true,
                    legend: {
                        display: false,
                        labels: {
                            fontColor: chartColors.fontColor
                        }
                    },
                    title: {
                        display: false,
                        fontColor: chartColors.fontColor
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                display: false,
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor,
                                display: false
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor,
                                min: -50,
                                max: 50
                            }
                        }],
                    }
                }
            });

        }
    }

    function chart_demo_4() {
        if ($('#chart_demo_4').length) {
            var ctx = document.getElementById("chart_demo_4").getContext("2d");
            var densityData = {
                backgroundColor: chartColors.primary.light,
                data: [10, 20, 40, 60, 80, 40, 60, 80, 40, 80, 20, 59]
            };
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
                    datasets: [densityData]
                },
                options: {
                    scaleFontColor: "#FFFFFF",
                    legend: {
                        display: false,
                        labels: {
                            fontColor: chartColors.fontColor
                        }
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor,
                                min: 0,
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
    }

    function chart_demo_5() {
        if ($('#chart_demo_5').length) {
            var ctx = document.getElementById('chart_demo_5').getContext('2d');
            window.myBar = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ['مهر', 'آبان', 'آذر', 'دی', 'بهمن'],
                    datasets: [
                        {
                            label: 'Dataset 1',
                            backgroundColor: [
                                chartColors.info.base,
                                chartColors.success.base,
                                chartColors.danger.base,
                                chartColors.dark,
                                chartColors.warning.base,
                            ],
                            yAxisID: 'y-axis-1',
                            data: [33, 56, -40, 25, 45]
                        },
                        {
                            label: 'Dataset 2',
                            backgroundColor: chartColors.info.base,
                            yAxisID: 'y-axis-2',
                            data: [23, 86, -40, 5, 45]
                        }
                    ]
                },
                options: {
                    legend: {
                        labels: {
                            fontColor: chartColors.fontColor
                        }
                    },
                    responsive: true,
                    title: {
                        display: true,
                        text: 'Chart.js Bar Chart - Multi Axis',
                        fontColor: chartColors.fontColor
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor
                            }
                        }],
                        yAxes: [
                            {
                                type: 'linear',
                                display: true,
                                position: 'left',
                                id: 'y-axis-1',
                            },
                            {
                                gridLines: {
                                    color: chartColors.borderColor
                                },
                                ticks: {
                                    fontColor: chartColors.fontColor
                                }
                            },
                            {
                                type: 'linear',
                                display: true,
                                position: 'right',
                                id: 'y-axis-2',
                                gridLines: {
                                    drawOnChartArea: false
                                },
                                ticks: {
                                    fontColor: chartColors.fontColor
                                }
                            }
                        ],
                    }
                }
            });
        }
    }

    function chart_demo_6() {
        if ($('#chart_demo_6').length) {
            var ctx = document.getElementById("chart_demo_6").getContext("2d");
            var speedData = {
                labels: ["0s", "10s", "20s", "30s", "40s", "50s", "60s"],
                datasets: [{
                    label: "Car Speed (mph)",
                    borderColor: chartColors.primary.base,
                    backgroundColor: 'rgba(0, 0, 0, 0',
                    data: [0, 59, 75, 20, 20, 55, 40]
                }]
            };
            var chartOptions = {
                legend: {
                    scaleFontColor: "#FFFFFF",
                    position: 'top',
                    labels: {
                        fontColor: chartColors.fontColor
                    }
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: chartColors.borderColor
                        },
                        ticks: {
                            fontColor: chartColors.fontColor
                        }
                    }],
                    yAxes: [{
                        gridLines: {
                            color: chartColors.borderColor
                        },
                        ticks: {
                            fontColor: chartColors.fontColor
                        }
                    }]
                }
            };
            new Chart(ctx, {
                type: 'line',
                data: speedData,
                options: chartOptions
            });
        }
    }

    function chart_demo_7() {
        if ($('#chart_demo_7').length) {
            var config = {
                type: 'pie',
                data: {
                    datasets: [{
                        borderWidth: 3,
                        borderColor: $('body').hasClass('dark') ? "#313852" : "rgba(255, 255, 255, 1)",
                        data: [
                            1242,
                            742,
                            442,
                            1742
                        ],
                        backgroundColor: [
                            colors.danger,
                            colors.info,
                            colors.warning,
                            colors.success
                        ],
                        label: 'Dataset 1'
                    }],
                    labels: [
                        'Organic Search',
                        'Email',
                        'Refferal',
                        'Social Media',
                    ]
                },
                options: {
                    responsive: true,
                    legend: {
                        display: false
                    }
                }
            };

            var ctx = document.getElementById('chart_demo_7').getContext('2d');
            new Chart(ctx, config);
        }
    }

    function chart_demo_8() {
        if ($('#chart_demo_8').length) {
            new Chart(document.getElementById("chart_demo_8"), {
                type: 'radar',
                data: {
                    labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
                    datasets: [
                        {
                            label: "1950",
                            fill: true,
                            backgroundColor: "rgba(179,181,198,0.2)",
                            borderColor: "rgba(179,181,198,1)",
                            pointBorderColor: "#fff",
                            pointBackgroundColor: "rgba(179,181,198,1)",
                            data: [-8.77, -55.61, 21.69, 6.62, 6.82]
                        }, {
                            label: "2050",
                            fill: true,
                            backgroundColor: "rgba(255,99,132,0.2)",
                            borderColor: "rgba(255,99,132,1)",
                            pointBorderColor: "#fff",
                            pointBackgroundColor: "rgba(255,99,132,1)",
                            data: [-25.48, 54.16, 7.61, 8.06, 4.45]
                        }
                    ]
                },
                options: {
                    legend: {
                        labels: {
                            fontColor: chartColors.fontColor
                        }
                    },
                    scale: {
                        gridLines: {
                            color: chartColors.borderColor
                        }
                    },
                    title: {
                        display: true,
                        text: 'Distribution in % of world population',
                        fontColor: chartColors.fontColor
                    }
                }
            });
        }
    }

    function chart_demo_9() {
        if ($('#chart_demo_9').length) {
            new Chart(document.getElementById("chart_demo_9"), {
                type: 'horizontalBar',
                data: {
                    labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
                    datasets: [
                        {
                            label: "Population (millions)",
                            backgroundColor: colors.primary,
                            data: [2478, 2267, 734, 1284, 1933]
                        }
                    ]
                },
                options: {
                    legend: {
                        display: false
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor,
                                display: false
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: chartColors.borderColor,
                                display: false
                            },
                            ticks: {
                                fontColor: chartColors.fontColor
                            },
                            barPercentage: 0.5
                        }]
                    }
                }
            });
        }
    }

    function chart_demo_10() {
        if ($('#chart_demo_10').length) {
            var element = document.getElementById("chart_demo_10");
            new Chart(element, {
                type: 'bar',
                data: {
                    labels: ["1900", "1950", "1999", "2050"],
                    datasets: [
                        {
                            label: "Europe",
                            type: "line",
                            borderColor: "#8e5ea2",
                            data: [408, 547, 675, 734],
                            fill: false
                        },
                        {
                            label: "Africa",
                            type: "line",
                            borderColor: "#3e95cd",
                            data: [133, 221, 783, 2478],
                            fill: false
                        },
                        {
                            label: "Europe",
                            type: "bar",
                            backgroundColor: chartColors.primary.base,
                            data: [408, 547, 675, 734],
                        },
                        {
                            label: "Africa",
                            type: "bar",
                            backgroundColor: chartColors.primary.light,
                            data: [133, 221, 783, 2478]
                        }
                    ]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Population growth (millions): Europe & Africa',
                        fontColor: chartColors.fontColor
                    },
                    legend: {
                        display: true,
                        labels: {
                            fontColor: chartColors.fontColor
                        }
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: chartColors.borderColor
                            },
                            ticks: {
                                fontColor: chartColors.fontColor
                            }
                        }]
                    }
                }
            });
        }
    }

    

    if ($('#sales-circle-graphic').length) {
        $('#sales-circle-graphic').circleProgress({
            startAngle: 1.55,
            value: 0.65,
            size: 180,
            thickness: 30,
            fill: {
                color: colors.primary
            }
        });
    }

    if ($('#circle-2').length) {
        $('#circle-2').circleProgress({
            startAngle: 1.55,
            value: 0.35,
            size: 90,
            thickness: 10,
            fill: {
                color: colors.warning
            }
        });
    }

    ////////////////////////////////////////////

    if ($(".dashboard-pie-1").length) {
        $(".dashboard-pie-1").peity("pie", {
            fill: [colors.primaryLight, colors.primary],
            radius: 30
        });
    }

    if ($(".dashboard-pie-2").length) {
        $(".dashboard-pie-2").peity("pie", {
            fill: [colors.successLight, colors.success],
            radius: 30
        });
    }

    if ($(".dashboard-pie-3").length) {
        $(".dashboard-pie-3").peity("pie", {
            fill: [colors.warningLight, colors.warning],
            radius: 30
        });
    }

    if ($(".dashboard-pie-4").length) {
        $(".dashboard-pie-4").peity("pie", {
            fill: [colors.infoLight, colors.info],
            radius: 30
        });
    }

    ////////////////////////////////////////////

 

   

    function users_chart() {
        Apex.chart = {
            fontFamily: 'inherit',
            locales: [{
                "name": "fa",
                "options": {
                    "months": ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهرویور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
                    "shortMonths": ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهرویور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
                    "days": ["یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"],
                    "shortDays": ["ی", "د", "س", "چ", "پ", "ج", "ش"],
                    "toolbar": {
                        "exportToSVG": "دریافت SVG",
                        "exportToPNG": "دریافت PNG",
                        "menu": "فهرست",
                        "selection": "انتخاب",
                        "selectionZoom": "بزرگنمایی قسمت انتخاب شده",
                        "zoomIn": "بزرگ نمایی",
                        "zoomOut": "کوچک نمایی",
                        "pan": "جا به جایی",
                        "reset": "بازنشانی بزرگ نمایی"
                    }
                }
            }],
            defaultLocale: "fa"
        }
        if ($('#users-chart').length > 0) {
            var lastDate = 0;
            var data = []
            var TICKINTERVAL = 86400000
            let XAXISRANGE = 777600000

            function getDayWiseTimeSeries(baseval, count, yrange) {
                var i = 0;
                while (i < count) {
                    var x = baseval;
                    var y = Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

                    data.push({
                        x, y
                    });
                    lastDate = baseval
                    baseval += TICKINTERVAL;
                    i++;
                }
            }

            getDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 10, {
                min: 10,
                max: 90
            });

            function getNewSeries(baseval, yrange) {
                var newDate = baseval + TICKINTERVAL;
                lastDate = newDate;

                for (var i = 0; i < data.length - 10; i++) {
                    // IMPORTANT
                    // we reset the x and y of the data which is out of drawing area
                    // to prevent memory leaks
                    data[i].x = newDate - XAXISRANGE - TICKINTERVAL
                    data[i].y = 0
                }

                data.push({
                    x: newDate,
                    y: Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min
                })

            }

            function resetData() {
                // Alternatively, you can also reset the data at certain intervals to prevent creating a huge series
                data = data.slice(data.length - 10, data.length);
            }

            var options = {
                chart: {
                    height: 270,
                    type: 'line',
                    animations: {
                        enabled: true,
                        easing: 'linear',
                        dynamicAnimation: {
                            speed: 1000
                        }
                    },
                    toolbar: {
                        show: false
                    },
                    zoom: {
                        enabled: false
                    }
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    curve: 'smooth',
                    width: 2
                },
                series: [{
                    data: data
                }],
                markers: {
                    size: 0
                },
                xaxis: {
                    type: 'datetime',
                    range: XAXISRANGE,
                },
                yaxis: {
                    max: 100
                },
                legend: {
                    show: false
                },
            }

            var chart = new ApexCharts(
                document.querySelector("#users-chart"),
                options
            );

            chart.render();

            window.setInterval(function () {
                getNewSeries(lastDate, {
                    min: 10,
                    max: 90
                })
                chart.updateSeries([{
                    data: data
                }])
            }, 1000)
        }
    }

    users_chart();

    function device_session_chart() {
        Apex.chart = {
            fontFamily: 'inherit',
            locales: [{
                "name": "fa",
                "options": {
                    "months": ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهرویور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
                    "shortMonths": ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهرویور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
                    "days": ["یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"],
                    "shortDays": ["ی", "د", "س", "چ", "پ", "ج", "ش"],
                    "toolbar": {
                        "exportToSVG": "دریافت SVG",
                        "exportToPNG": "دریافت PNG",
                        "menu": "فهرست",
                        "selection": "انتخاب",
                        "selectionZoom": "بزرگنمایی قسمت انتخاب شده",
                        "zoomIn": "بزرگ نمایی",
                        "zoomOut": "کوچک نمایی",
                        "pan": "جا به جایی",
                        "reset": "بازنشانی بزرگ نمایی"
                    }
                }
            }],
            defaultLocale: "fa"
        }
        if ($('#device_session_chart').length) {
            var options = {
                chart: {
                    type: 'area',
                    stacked: true,
                    events: {
                        selection: function (chart, e) {
                            // console.log(new Date(e.xaxis.min))
                        }
                    },
                    toolbar: {
                        show: false,
                    }

                },
                colors: ['#008FFB', '#00E396', '#CED4DC'],
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    curve: 'smooth',
                    width: 1
                },
                series: [{
                    name: 'شرق',
                    data: generateDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 20, {
                        min: 10,
                        max: 60
                    })
                },
                {
                    name: 'شمال',
                    data: generateDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 20, {
                        min: 10,
                        max: 20
                    })
                },

                {
                    name: 'جنوب',
                    data: generateDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 20, {
                        min: 10,
                        max: 15
                    })
                }
                ],
                fill: {
                    type: 'gradient',
                    gradient: {
                        opacityFrom: 0.6,
                        opacityTo: 0,
                    }
                },
                legend: {
                    show: false,
                    position: 'top',
                    horizontalAlign: 'left'
                },
                xaxis: {
                    type: 'datetime'
                },
            };

            var chart = new ApexCharts(
                document.querySelector("#device_session_chart"),
                options
            );

            chart.render();

            /*
              // this function will generate output in this format
              // data = [
                  [timestamp, 23],
                  [timestamp, 33],
                  [timestamp, 12]
                  ...
              ]
              */
            function generateDayWiseTimeSeries(baseval, count, yrange) {
                var i = 0;
                var series = [];
                while (i < count) {
                    var x = baseval;
                    var y = Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

                    series.push([x, y]);
                    baseval += 86400000;
                    i++;
                }
                return series;
            }
        }
    }

    device_session_chart();

    function chart1() {
        if ($('#chart1').length) {
            var options = {
                chart: {
                    type: 'bar',
                    toolbar: {
                        show: false
                    }
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                        columnWidth: '55%',
                        backgroundBarColors: ['red']
                    },
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    show: true,
                    width: 1,
                    colors: ['transparent']
                },
                colors: [colors.secondary, colors.info, colors.warning],
                series: [{
                    name: 'بدون سود',
                    data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
                }, {
                    name: 'درآمد',
                    data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
                }, {
                    name: 'مالیات',
                    data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
                }],
                xaxis: {
                    categories: ['مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند', 'فروردین', 'اردیبهشت', 'خرداد'],
                },
                legend: {
                    position: 'bottom',
                    offsetY: -10
                },
                tooltip: {
                    y: {
                        formatter: function (val) {
                            return "$ " + val + " thousands"
                        }
                    }
                }
            };

            var chart = new ApexCharts(
                document.querySelector("#chart1"),
                options
            );

            chart.render();
        }
    }

    chart1();

    function widget_chart1() {
        if ($('#widget-chart1').length) {
            var ctx = document.getElementById("widget-chart1");
            ctx.height = 50;
            new Chart(ctx.getContext('2d'), {
                type: 'line',
                data: {
                    labels: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sst", "Sun"],
                    datasets: [{
                        label: 'data-2',
                        data: [5, 15, 5, 20, 5, 15, 5],
                        backgroundColor: "rgba(0,0,255,0)",
                        borderWidth: 1,
                        borderColor: colors.success,
                        pointBorder: false,
                    }]
                },
                options: {
                    elements: {
                        point: {
                            radius: 0
                        }
                    },
                    tooltips: {
                        enabled: false
                    },
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            display: false,
                        }],
                        xAxes: [{
                            display: false
                        }]
                    },
                }
            });
        }
    }

    widget_chart1();

    function widget_chart2() {
        if ($('#widget-chart2').length) {
            var ctx = document.getElementById("widget-chart2");
            ctx.height = 50;
            var barChart = new Chart(ctx.getContext('2d'), {
                type: 'line',
                data: {
                    labels: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sst", "Sun"],
                    datasets: [{
                        label: 'data-2',
                        data: [5, 10, 10, 10, 5, 15, 10],
                        backgroundColor: "rgba(0,0,255,0)",
                        borderColor: colors.warning,
                        borderWidth: 1,
                        pointBorder: false,
                    }]
                },
                options: {
                    elements: {
                        point: {
                            radius: 0
                        }
                    },
                    tooltips: {
                        enabled: false
                    },
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            display: false
                        }],
                        xAxes: [{
                            display: false
                        }]
                    },
                }
            });
        }
    }

    widget_chart2();

    function widget_chart3() {
        if ($('#widget-chart3').length) {
            var ctx = document.getElementById("widget-chart3");
            ctx.height = 50;
            var barChart = new Chart(ctx.getContext('2d'), {
                type: 'line',
                data: {
                    labels: ["شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه"],
                    datasets: [{
                        label: 'data-2',
                        data: [10, 5, 15, 5, 15, 5, 15],
                        backgroundColor: "rgba(0,0,255,0)",
                        borderColor: colors.danger,
                        borderWidth: 1,
                        pointBorder: false,
                    }]
                },
                options: {
                    elements: {
                        point: {
                            radius: 0
                        }
                    },
                    tooltips: {
                        enabled: false
                    },
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            display: false
                        }],
                        xAxes: [{
                            display: false
                        }]
                    },
                }
            });
        }
    }

    widget_chart3();

    function contactsStatuses() {
        if ($('#contacts-statuses').length) {
            var chart = new ApexCharts(
                document.querySelector("#contacts-statuses"), {
                chart: {
                    width: "100%",
                    type: 'donut',
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    width: 3,
                    colors: $('body').hasClass('dark') ? "#313852" : "rgba(255, 255, 255, 1)",
                },
                series: [44, 55, 13, 33],
                labels: ['کاربر جدید', 'مورد 1', 'مورد 2', 'مورد 3'],
                colors: [colors.warning, colors.info, colors.success, colors.danger],
                legend: {
                    position: 'bottom',
                }
            }
            );

            chart.render();
        }
    }

    contactsStatuses();

    function numberOfOrders() {
        //if ($('#number-of-orders').length) {
        //    var ts2 = 1484418600000;
        //    var dates = [];
        //    for (var i = 0; i < 120; i++) {
        //        ts2 = ts2 + 86400000;
        //        var innerArr = [ts2, dataSeries[1][i].value];
        //        dates.push(innerArr)
        //    }

        //    var options = {
        //        chart: {
        //            type: 'area',
        //            stacked: false,
        //            height: 350,
        //            toolbar: {
        //                show: false
        //            }
        //        },
        //        dataLabels: {
        //            enabled: false
        //        },
        //        series: [{
        //            name: 'تعداد سفارشات',
        //            data: dates
        //        }],
        //        stroke: {
        //            colors: [colors.primary],
        //            width: 1
        //        },
        //        markers: {
        //            size: 0,
        //            strokeColors: 'white',
        //            colors: colors.primary
        //        },
        //        fill: {
        //            type: 'gradient',
        //            gradient: {
        //                opacityFrom: 0.6,
        //                opacityTo: 0,
        //            }
        //        },
        //        yaxis: {
        //            labels: {
        //                formatter: function (val) {
        //                    return (val / 1000000).toFixed(0);
        //                },
        //            },
        //            title: {
        //                text: 'قیمت'
        //            },
        //        },
        //        xaxis: {
        //            type: 'datetime',
        //        },

        //        tooltip: {
        //            shared: false,
        //            y: {
        //                formatter: function (val) {
        //                    return (val / 1000000).toFixed(0)
        //                }
        //            }
        //        }
        //    };

        //    var chart = new ApexCharts(
        //        document.querySelector("#number-of-orders"),
        //        options
        //    );

        //    chart.render();
        //}
    }

    numberOfOrders();

    function hotProducts() {
        if ($('#hot-products').length) {
            var randomScalingFactor = function () {
                return Math.round(Math.random() * 100);
            };

            var config = {
                type: 'pie',
                data: {
                    datasets: [{
                        data: [
                            randomScalingFactor(),
                            randomScalingFactor(),
                            randomScalingFactor(),
                            randomScalingFactor(),
                            randomScalingFactor(),
                        ],
                        backgroundColor: [
                            colors.primary,
                            colors.success,
                            colors.info,
                            colors.secondary,
                            colors.warning,
                        ],
                        borderColor: $('body').hasClass('dark') ? chartColors.dark : 'white',
                        label: 'Dataset 1'
                    }],
                    labels: [
                        'اپل آیفون ایکس آر 256 GB قرمز',
                        'سامسونگ گلکسی آ 30 3/32 GB آبی',
                        'Apple iPhone XS 64GB gold',
                        'سامسونگ گلکسی نوت 9 9 6/128GB',
                        'اپل آیفون ایکس آر 256 GB قرمز'
                    ]
                },
                options: {
                    legend: {
                        display: false
                    },
                    responsive: true,
                    legendCallback: function (chart) {
                        var text = [];
                        text.push('<ul class="' + chart.id + '-legend">');
                        for (var i = 0; i < chart.legend.legendItems.length; i++) {
                            text.push('<li><div class="legendValue"><span style="background-color:' + chart.legend.legendItems[i].fillStyle + '">&nbsp;&nbsp;&nbsp;&nbsp;</span>');

                            if (chart.legend.legendItems[i].text) {
                                text.push('<span class="label ml-2">' + chart.legend.legendItems[i].text + '</span>');
                            }

                            text.push('</div></li><div class="clear"></div>');
                        }

                        text.push('</ul>');

                        return text.join('');
                    },
                },
            };


            window.onload = function () {
                var ctx = document.getElementById('hot-products').getContext('2d');

                var chart = new Chart(ctx, config);

                document.getElementById('hot-products-legends').innerHTML = chart.generateLegend();
            };
        }
    }

    hotProducts();

    //function revenue() {
    //    if ($('#revenue').length) {
    //        var options = {
    //            chart: {
    //                type: 'line',
    //                zoom: {
    //                    enabled: false
    //                },
    //                toolbar: {
    //                    show: false
    //                }
    //            },
    //            series: [{
    //                name: "دسکتاپ",
    //                data: [10, 41, 35, 51, 49, 62, 69, 91, 148]
    //            }],
    //            dataLabels: {
    //                enabled: false
    //            },
    //            stroke: {
    //                curve: 'straight',
    //                colors: [colors.primary]
    //            },
    //            markers: {
    //                strokeColors: 'white',
    //                colors: colors.primary
    //            },
    //            xaxis: {
    //                categories: ['مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند', 'فروردین', 'ادریبهشت', 'خرداد'],
    //            }
    //        };

    //        var chart = new ApexCharts(
    //            document.querySelector("#revenue"),
    //            options
    //        );

    //        chart.render();
    //    }
    //}

    //revenue();

    function projectTasks() {
        if ($('#project-tasks').length) {
            var options = {
                colors: [colors.primary, colors.success, colors.info, colors.warning],
                chart: {
                    height: 362,
                    type: 'bar',
                    stacked: true,
                    toolbar: {
                        show: false
                    },
                    zoom: {
                        enabled: false
                    }
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                    },
                },
                series: [{
                    name: 'پروژه آ',
                    data: [44, 55, 41, 67, 22, 43]
                }, {
                    name: 'پروژه ب',
                    data: [13, 23, 20, 8, 13, 27]
                }, {
                    name: 'پروژه س',
                    data: [11, 17, 15, 15, 21, 14]
                }, {
                    name: 'پروژه د',
                    data: [21, 7, 25, 13, 22, 8]
                }],
                xaxis: {
                    type: 'datetime',
                    categories: ['01/01/2011 GMT', '01/02/2011 GMT', '01/03/2011 GMT', '01/04/2011 GMT', '01/05/2011 GMT', '01/06/2011 GMT'],
                },
                legend: {
                    position: 'bottom',
                    offsetY: -10
                },
                fill: {
                    opacity: 1
                },
            };

            var chart = new ApexCharts(
                document.querySelector("#project-tasks"),
                options
            );

            chart.render();
        }
    }

    projectTasks();


    // ==================
    var label = [];
    var data = [];

    const initCircleChart = (a, b) => {
        label = a;
        data = b;
    }


    //var canvas = document.getElementById('canvas');
    //var ctx = canvas.getContext('2d');

    //var myChart = new Chart(ctx, {
    //    type: 'pie',

    //    options: {
    //        responsive: true,
    //        legend: {
    //            position: 'bottom',
    //        },
    //    },
    //    data: {
    //        labels: label,

    //        datasets: [{
    //            label: "",
    //            borderWidth: 5,
    //            backgroundColor: [
    //                colors.primary,
    //                colors.secondary,
    //                colors.success,
    //                colors.warning,
    //                colors.info
    //            ],
    //            data: data
    //        }]
    //    }
    //});


});

