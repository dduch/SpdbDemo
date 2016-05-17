var CONST = {
    WarsawLatitude: 52.229833,
    WarsawLongitude: 21.011734,
    DefaultZoom: 12,
    NominatimSearch: 'http://nominatim.openstreetmap.org/search/',
    NominatimReverse: 'http://nominatim.openstreetmap.org/reverse?format=json&',
    FormatType: '?format=json',
    stations: {
        "Stations": [
          {
              "Localization": "1. Citi",
              "Station number": 6081,
              "Latitude": 52.18091213826019,
              "Longitude": 20.991702675819397,
              "District": "Mokotów"
          },
          {
              "Localization": "2. Nestle House",
              "Station number": 6082,
              "Latitude": 52.1839973961197,
              "Longitude": 21.009796857833862,
              "District": "Mokotów"
          },
          {
              "Localization": "New City",
              "Station number": 6083,
              "Latitude": 52.177803639639116,
              "Longitude": 20.997496247291565,
              "District": "Mokotów"
          },
          {
              "Localization": "Starcom MediaVest Group",
              "Station number": 6084,
              "Latitude": 52.18342838278314,
              "Longitude": 21.00187361240387,
              "District": "Mokotów"
          },
          {
              "Localization": "The Park Warsaw",
              "Station number": 6085,
              "Latitude": 52.18933853050738,
              "Longitude": 20.945236086845398,
              "District": "Włochy"
          },
          {
              "Localization": "Warsaw Spire",
              "Station number": 6086,
              "Latitude": 52.232543907723425,
              "Longitude": 20.983328819274902,
              "District": "Wola"
          },
          {
              "Localization": "Trinity Park",
              "Station number": 6087,
              "Latitude": 52.1835270561379,
              "Longitude": 20.990527868270874,
              "District": "Mokotów"
          },
          {
              "Localization": "Trinity Park 1",
              "Station number": 60871,
              "Latitude": 52.18345798481257,
              "Longitude": 20.990538597106934,
              "District": "Mokotów"
          },
          {
              "Localization": "Galeria Bemowo",
              "Station number": 6213,
              "Latitude": 52.2645854944588,
              "Longitude": 20.9312510490417,
              "District": "Bemowo"
          },
          {
              "Localization": "ul. Słomińskiego – Metro Dw. Gdański",
              "Station number": 6300,
              "Latitude": 52.25764445766444,
              "Longitude": 20.994502902030945,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Słomińskiego – Metro Dw. Gdański 1",
              "Station number": 63001,
              "Latitude": 52.25751639616425,
              "Longitude": 20.99452972412109,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Andersa – ul. Muranowska",
              "Station number": 6301,
              "Latitude": 52.2541078541912,
              "Longitude": 20.998107790947,
              "District": "Śródmieście"
          },
          {
              "Localization": "Metro Ratusz Arsenał Południowy",
              "Station number": 6302,
              "Latitude": 52.244573710696,
              "Longitude": 20.9999907016754,
              "District": "Śródmieście"
          },
          {
              "Localization": "pl. Bankowy, rejon Ratusza",
              "Station number": 6303,
              "Latitude": 52.24308248127586,
              "Longitude": 21.001771688461304,
              "District": "Śródmieście"
          },
          {
              "Localization": "pl. Bankowy",
              "Station number": 6304,
              "Latitude": 52.24423868113828,
              "Longitude": 21.00161612033844,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Andersa, rejon Pl. Bankowego i ul. Długiej",
              "Station number": 6305,
              "Latitude": 52.24491859142194,
              "Longitude": 21.001262068748474,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Senatorska – Bielańska",
              "Station number": 6306,
              "Latitude": 52.243516059754675,
              "Longitude": 21.006712317466736,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Marszałkowska – ul. Królewska",
              "Station number": 6307,
              "Latitude": 52.23848037586037,
              "Longitude": 21.006744503974915,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Królewska – Plac Małachowskiego",
              "Station number": 6308,
              "Latitude": 52.23965640536594,
              "Longitude": 21.01184606552124,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Bracka – ul. Krucza",
              "Station number": 6309,
              "Latitude": 52.23196894473187,
              "Longitude": 21.016164422035217,
              "District": "Śródmieście"
          },
          {
              "Localization": "al. Jerozolimskie, rejon rotundy",
              "Station number": 6310,
              "Latitude": 52.230217725853215,
              "Longitude": 21.01259171962738,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Nowy Świat – Rondo de Gaulle´a",
              "Station number": 6311,
              "Latitude": 52.2314251871654,
              "Longitude": 21.0213263332844,
              "District": "Śródmieście"
          },
          {
              "Localization": "al. 3-go Maja, rejon Dw. Powiśle",
              "Station number": 6312,
              "Latitude": 52.23388436394939,
              "Longitude": 21.030020713806152,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Topiel – ul. Tamka",
              "Station number": 6313,
              "Latitude": 52.2375474140858,
              "Longitude": 21.0257023572922,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Oboźna – ul. Kopernika",
              "Station number": 6314,
              "Latitude": 52.23831283839485,
              "Longitude": 21.018412113189697,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Bonifraterska – Plac Krasińskich",
              "Station number": 6315,
              "Latitude": 52.24884675795982,
              "Longitude": 21.005457043647766,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Miodowa",
              "Station number": 6316,
              "Latitude": 52.24627509626952,
              "Longitude": 21.01130962371826,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Anielewicza – ul. Andersa",
              "Station number": 6317,
              "Latitude": 52.24920802671565,
              "Longitude": 20.997206568717957,
              "District": "Śródmieście"
          },
          {
              "Localization": "al. Jana Pawła II – al. Solidarności",
              "Station number": 6318,
              "Latitude": 52.24219560301015,
              "Longitude": 20.992990136146545,
              "District": "Wola"
          },
          {
              "Localization": "al. Jana Pawła II – pl. Mirowski",
              "Station number": 6319,
              "Latitude": 52.238608492319024,
              "Longitude": 20.995774269104004,
              "District": "Wola"
          },
          {
              "Localization": "ul. Grzybowska – Al. Jana Pawła II",
              "Station number": 6320,
              "Latitude": 52.235707713687624,
              "Longitude": 20.996793508529663,
              "District": "Wola"
          },
          {
              "Localization": "al. Jana Pawła II – ul. Stawki",
              "Station number": 6321,
              "Latitude": 52.25138542973338,
              "Longitude": 20.985780358314514,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Krucza – ul. Wspólna",
              "Station number": 6322,
              "Latitude": 52.22820028864685,
              "Longitude": 21.017950773239136,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Traugutta – ul. Krakowskie Przedmieście",
              "Station number": 6323,
              "Latitude": 52.23935090349061,
              "Longitude": 21.01681351661682,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Traugutta – ul. Krakowskie Przedmieście 1",
              "Station number": 63231,
              "Latitude": 52.23927206395559,
              "Longitude": 21.01672768592834,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Traugutta – ul. Krakowskie Przedmieście 2",
              "Station number": 63232,
              "Latitude": 52.23927863392218,
              "Longitude": 21.016899347305298,
              "District": "Śródmieście"
          },
          {
              "Localization": "Sadyba Best Mall",
              "Station number": 6324,
              "Latitude": 52.18735212442676,
              "Longitude": 21.061735153198242,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Świętokrzyska – Sezam",
              "Station number": 6325,
              "Latitude": 52.235162359274504,
              "Longitude": 21.008772253990173,
              "District": "Śródmieście"
          },
          {
              "Localization": "al. Jana Pawła II – ul. Anielewicza",
              "Station number": 6326,
              "Latitude": 52.24710277377028,
              "Longitude": 20.989433526992794,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Gałczyńskiego – ul. Foksal",
              "Station number": 6327,
              "Latitude": 52.23365110119173,
              "Longitude": 21.02034866809845,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Kościelna – Nowe Miasto",
              "Station number": 6328,
              "Latitude": 52.254117705846,
              "Longitude": 21.0093784332275,
              "District": "Śródmieście"
          },
          {
              "Localization": "Wybrzeże Kościuszkowskie – Centrum Nauki Kopernik",
              "Station number": 6329,
              "Latitude": 52.24129885251972,
              "Longitude": 21.028132438659668,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Solec przy al.3-go Maja",
              "Station number": 6330,
              "Latitude": 52.23462028348882,
              "Longitude": 21.034199595451355,
              "District": "Śródmieście"
          },
          {
              "Localization": "Metro Stare Bielany ul. Kasprowicza",
              "Station number": 6331,
              "Latitude": 52.2816642231669,
              "Longitude": 20.94968855381012,
              "District": "Bielany"
          },
          {
              "Localization": "Metro Stare Bielany ul. Kasprowicza 1",
              "Station number": 63311,
              "Latitude": 52.28153623103404,
              "Longitude": 20.94995141029358,
              "District": "Bielany"
          },
          {
              "Localization": "Metro Słodowiec",
              "Station number": 6332,
              "Latitude": 52.2760716067128,
              "Longitude": 20.9617102146149,
              "District": "Bielany"
          },
          {
              "Localization": "Metro Słodowiec 1",
              "Station number": 63321,
              "Latitude": 52.276245566098574,
              "Longitude": 20.961399078369137,
              "District": "Bielany"
          },
          {
              "Localization": "Radiowa – Fontanna",
              "Station number": 6333,
              "Latitude": 52.252567685245424,
              "Longitude": 20.91468572616577,
              "District": "Bemowo"
          },
          {
              "Localization": "Pętla Os. Górczewska",
              "Station number": 6334,
              "Latitude": 52.239189939291,
              "Longitude": 20.8987534046173,
              "District": "Bemowo"
          },
          {
              "Localization": "Urząd Dzielnicy Bielany – ul. Żeromskiego",
              "Station number": 6335,
              "Latitude": 52.2768232376509,
              "Longitude": 20.9479880332947,
              "District": "Bielany"
          },
          {
              "Localization": "ul. Broniewskiego – ul. Perzyńskiego",
              "Station number": 6336,
              "Latitude": 52.27054722055992,
              "Longitude": 20.95041275024414,
              "District": "Bielany"
          },
          {
              "Localization": "ul. Conrada",
              "Station number": 6337,
              "Latitude": 52.2710396180402,
              "Longitude": 20.930580496788,
              "District": "Bielany"
          },
          {
              "Localization": "Metro Ursynów – ul. Beli Bartoka I",
              "Station number": 6338,
              "Latitude": 52.1622513061534,
              "Longitude": 21.0280895233154,
              "District": "Ursynów"
          },
          {
              "Localization": "Metro Ursynów – ul. Beli Bartoka I 1",
              "Station number": 63381,
              "Latitude": 52.1621525855968,
              "Longitude": 21.0280680656433,
              "District": "Ursynów"
          },
          {
              "Localization": "Metro Ursynów – ul. Beli Bartoka II",
              "Station number": 6339,
              "Latitude": 52.1617692186922,
              "Longitude": 21.0276791453362,
              "District": "Ursynów"
          },
          {
              "Localization": "Metro Ursynów – ul. Beli Bartoka II 1",
              "Station number": 63391,
              "Latitude": 52.1617741547678,
              "Longitude": 21.0278052091599,
              "District": "Ursynów"
          },
          {
              "Localization": "Metro Stokłosy – al. Komisji Edukacji Narodowej",
              "Station number": 6340,
              "Latitude": 52.156887171951,
              "Longitude": 21.0337328910828,
              "District": "Ursynów"
          },
          {
              "Localization": "SGGW I",
              "Station number": 6341,
              "Latitude": 52.16234344514205,
              "Longitude": 21.038432121276852,
              "District": "Ursynów"
          },
          {
              "Localization": "SGGW I 1",
              "Station number": 63411,
              "Latitude": 52.16244874660971,
              "Longitude": 21.03833556175232,
              "District": "Ursynów"
          },
          {
              "Localization": "SGGW II",
              "Station number": 6342,
              "Latitude": 52.1572557711822,
              "Longitude": 21.0439360141754,
              "District": "Ursynów"
          },
          {
              "Localization": "SGGW II 1",
              "Station number": 63421,
              "Latitude": 52.1571208378892,
              "Longitude": 21.0442686080933,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Dereniowa – ul. Płaskowickiej",
              "Station number": 6343,
              "Latitude": 52.1427217636099,
              "Longitude": 21.0448050498962,
              "District": "Ursynów"
          },
          {
              "Localization": "Metro Imielin – Ratusz Ursynów",
              "Station number": 6344,
              "Latitude": 52.1492051326616,
              "Longitude": 21.0454595088959,
              "District": "Ursynów"
          },
          {
              "Localization": "Metro Imielin – Ratusz Ursynów 1",
              "Station number": 63441,
              "Latitude": 52.1492561531476,
              "Longitude": 21.0453656315804,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Pileckiego – ul. Alternatywy",
              "Station number": 6345,
              "Latitude": 52.14267402776558,
              "Longitude": 21.038373112678528,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Ciszewskiego – ul. Pileckiego",
              "Station number": 6346,
              "Latitude": 52.1494684635744,
              "Longitude": 21.0332715511322,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Puławska – ul. Pileckiego",
              "Station number": 6347,
              "Latitude": 52.1520885213144,
              "Longitude": 21.0181519389153,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Romera – ul. Pieciolinii",
              "Station number": 6348,
              "Latitude": 52.1557303429351,
              "Longitude": 21.0233715176582,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Rosoła – ul. Gandhi",
              "Station number": 6349,
              "Latitude": 52.1548532391624,
              "Longitude": 21.0493111610413,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Rosoła – ul. Wesoła",
              "Station number": 6350,
              "Latitude": 52.1521740984895,
              "Longitude": 21.0563278198242,
              "District": "Ursynów"
          },
          {
              "Localization": "UKSW – ul. Wóycickiego",
              "Station number": 6351,
              "Latitude": 52.3125782570109,
              "Longitude": 20.9176898002625,
              "District": "Bielany"
          },
          {
              "Localization": "UKSW – ul. Wóycickiego 1",
              "Station number": 63511,
              "Latitude": 52.3126307298567,
              "Longitude": 20.9178936481476,
              "District": "Bielany"
          },
          {
              "Localization": "UKSW II",
              "Station number": 6352,
              "Latitude": 52.2951013398688,
              "Longitude": 20.9594786167145,
              "District": "Bielany"
          },
          {
              "Localization": "UKSW II 1",
              "Station number": 63521,
              "Latitude": 52.295048846244704,
              "Longitude": 20.959478616714478,
              "District": "Bielany"
          },
          {
              "Localization": "Central Park Ursynów",
              "Station number": 6353,
              "Latitude": 52.163310893005075,
              "Longitude": 20.992426872253418,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Wybrzeże Szczecińskie – Stadion Narodowy",
              "Station number": 6354,
              "Latitude": 52.237166339550825,
              "Longitude": 21.043753623962402,
              "District": "Praga Płd."
          },
          {
              "Localization": "ul. Ks. Hlonda",
              "Station number": 6355,
              "Latitude": 52.16008598500546,
              "Longitude": 21.071643233299252,
              "District": "Wilanów"
          },
          {
              "Localization": "ul. Przyczółkowa – Al. Wilanowska",
              "Station number": 6356,
              "Latitude": 52.1654299910244,
              "Longitude": 21.0839921236038,
              "District": "Wilanów"
          },
          {
              "Localization": "Arkadia",
              "Station number": 6357,
              "Latitude": 52.2559960480504,
              "Longitude": 20.9833288192749,
              "District": "Śródmieście"
          },
          {
              "Localization": "al. Jerozolimskie / ul.E. Plater (Mariott)",
              "Station number": 6358,
              "Latitude": 52.2280425698225,
              "Longitude": 21.0049796104431,
              "District": "Śródmieście"
          },
          {
              "Localization": "Plac Politechniki – ul. Nowowiejska",
              "Station number": 6359,
              "Latitude": 52.21984041887858,
              "Longitude": 21.011738777160645,
              "District": "Śródmieście"
          },
          {
              "Localization": "Plac Politechniki – ul. Nowowiejska 1",
              "Station number": 63591,
              "Latitude": 52.21984041887858,
              "Longitude": 21.011695861816406,
              "District": "Śródmieście"
          },
          {
              "Localization": "Trasa Armii Ludowej, rejon Placu na Rozdrożu",
              "Station number": 6360,
              "Latitude": 52.2199094336799,
              "Longitude": 21.0256862640381,
              "District": "Śródmieście"
          },
          {
              "Localization": "Metro Politechnika – Rondo Jazdy Polskiej",
              "Station number": 6361,
              "Latitude": 52.2173886806455,
              "Longitude": 21.0140991210938,
              "District": "Śródmieście"
          },
          {
              "Localization": "Plac Trzech Krzyży",
              "Station number": 6362,
              "Latitude": 52.227838848845245,
              "Longitude": 21.023545861244198,
              "District": "Śródmieście"
          },
          {
              "Localization": "Plac Konstytucji",
              "Station number": 6363,
              "Latitude": 52.2228933996631,
              "Longitude": 21.0157929360867,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Waryńskiego – ul. Nowowiejska – Metro Politechnika",
              "Station number": 6364,
              "Latitude": 52.2197311452238,
              "Longitude": 21.0157366096973,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Marszałkowska, rejon ul. Złotej",
              "Station number": 6365,
              "Latitude": 52.2328855964877,
              "Longitude": 21.0101723670959,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Noakowskiego – ul.Koszykowa",
              "Station number": 6366,
              "Latitude": 52.2225935332893,
              "Longitude": 21.0081821680069,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Marszałkowska – Al. Jerozolimskie",
              "Station number": 6367,
              "Latitude": 52.2293330547814,
              "Longitude": 21.0113243758678,
              "District": "Śródmieście"
          },
          {
              "Localization": "Metro Stadion Narodowy",
              "Station number": 6368,
              "Latitude": 52.247730090478704,
              "Longitude": 21.043018698692322,
              "District": "Praga Płn."
          },
          {
              "Localization": "ul. Floriańska – rejon Bazyliki Św. Floriana",
              "Station number": 6369,
              "Latitude": 52.25244946111211,
              "Longitude": 21.030390858650208,
              "District": "Praga Płn."
          },
          {
              "Localization": "ul. Wałbrzyska Al. KEN – Metro Służew",
              "Station number": 6370,
              "Latitude": 52.173224376111214,
              "Longitude": 21.025707721710205,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Wybrzeże Helskie – ZOO – Wejście PN",
              "Station number": 6371,
              "Latitude": 52.26126286311156,
              "Longitude": 21.015660166740414,
              "District": "Praga Płn."
          },
          {
              "Localization": "ul. Kijowska – Dworzec Wschodni",
              "Station number": 6372,
              "Latitude": 52.25273845288172,
              "Longitude": 21.051108241081238,
              "District": "Praga Płn."
          },
          {
              "Localization": "Aleja Zieleniecka – Teatr Powszechny",
              "Station number": 6373,
              "Latitude": 52.2471881681893,
              "Longitude": 21.0476079583168,
              "District": "Praga Płd."
          },
          {
              "Localization": "Rondo Waszyngtona – Stadion Narodowy",
              "Station number": 6374,
              "Latitude": 52.2386479126934,
              "Longitude": 21.0507220029831,
              "District": "Praga Płd."
          },
          {
              "Localization": "Rondo Waszyngtona – Stadion Narodowy 1",
              "Station number": 63741,
              "Latitude": 52.23871689826441,
              "Longitude": 21.05082929134369,
              "District": "Praga Płd."
          },
          {
              "Localization": "al. Jerozolimskie – Metro Centrum",
              "Station number": 6375,
              "Latitude": 52.2300189433596,
              "Longitude": 21.0109367966652,
              "District": "Śródmieście"
          },
          {
              "Localization": "Plac Zbawiciela – Nowowiejska – Mokotowska",
              "Station number": 6376,
              "Latitude": 52.2197533286191,
              "Longitude": 21.0174143314362,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Świętokrzyska – Rejon Ronda ONZ",
              "Station number": 6377,
              "Latitude": 52.23278046176347,
              "Longitude": 20.997651815414425,
              "District": "Śródmieście"
          },
          {
              "Localization": "Oczki – Akademia Medyczna",
              "Station number": 6378,
              "Latitude": 52.2248486430822,
              "Longitude": 21.00315570831299,
              "District": "Śródmieście"
          },
          {
              "Localization": "al. Jerozolimskie – Plac Zawiszy",
              "Station number": 6379,
              "Latitude": 52.2248486430822,
              "Longitude": 20.9897929430008,
              "District": "Ochota"
          },
          {
              "Localization": "ul. Raszyńska – Pomnik Lotnika",
              "Station number": 6380,
              "Latitude": 52.21654072895435,
              "Longitude": 20.987995862960815,
              "District": "Ochota"
          },
          {
              "Localization": "ul. Poznańska – Zespół Szkół Gastronomicznych",
              "Station number": 6381,
              "Latitude": 52.2235070924771,
              "Longitude": 21.0128907859325,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Marszałkowska – Metro Świętokrzyska",
              "Station number": 6382,
              "Latitude": 52.2363614730989,
              "Longitude": 21.00733995437622,
              "District": "Śródmieście"
          },
          {
              "Localization": "Plac Wileński",
              "Station number": 6383,
              "Latitude": 52.255020764910995,
              "Longitude": 21.03385627269745,
              "District": "Praga Płn."
          },
          {
              "Localization": "ul. Ratuszowa – ZOO – wejście PD",
              "Station number": 6384,
              "Latitude": 52.25373348968678,
              "Longitude": 21.02287530899048,
              "District": "Praga Płn."
          },
          {
              "Localization": "pl. Hallera – Dąbrowszczaków",
              "Station number": 6385,
              "Latitude": 52.26140404717611,
              "Longitude": 21.03057861328125,
              "District": "Praga Płn."
          },
          {
              "Localization": "ul. Międzynarodowa – ul. Walecznych",
              "Station number": 6386,
              "Latitude": 52.2365388734892,
              "Longitude": 21.0648250579834,
              "District": "Praga Płd."
          },
          {
              "Localization": "ul. Francuska – Walecznych",
              "Station number": 6387,
              "Latitude": 52.23469913128726,
              "Longitude": 21.05358123779297,
              "District": "Praga Płd."
          },
          {
              "Localization": "ul. Lipowa – Dobra",
              "Station number": 6388,
              "Latitude": 52.2417948596094,
              "Longitude": 21.0241627693176,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Lipowa – Dobra 1",
              "Station number": 63881,
              "Latitude": 52.24188026424411,
              "Longitude": 21.024232506752014,
              "District": "Śródmieście"
          },
          {
              "Localization": "pl. Grzybowski – Próżna",
              "Station number": 6389,
              "Latitude": 52.2358481577903,
              "Longitude": 21.0038611292839,
              "District": "Śródmieście"
          },
          {
              "Localization": "pl. Unii Lubelskiej – Puławska",
              "Station number": 6390,
              "Latitude": 52.2131660166141,
              "Longitude": 21.0211506485939,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Myśliwiecka – LO Stefana Batorego",
              "Station number": 6391,
              "Latitude": 52.2220669137432,
              "Longitude": 21.0343900322914,
              "District": "Śródmieście"
          },
          {
              "Localization": "Ratusz Bemowo",
              "Station number": 6392,
              "Latitude": 52.238782598708504,
              "Longitude": 20.91397225856781,
              "District": "Bemowo"
          },
          {
              "Localization": "Rondo Starzyńskiego",
              "Station number": 6393,
              "Latitude": 52.26312776985749,
              "Longitude": 21.021416187286373,
              "District": "Praga Płn."
          },
          {
              "Localization": "ul. Emilii Plater – Świętokrzyska",
              "Station number": 6394,
              "Latitude": 52.2337463776774,
              "Longitude": 21.002790927887,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Emilii Plater – Al. Jerozolimskie",
              "Station number": 6395,
              "Latitude": 52.22915973275686,
              "Longitude": 21.005381941795346,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Lindleya – Al. Jerozolimskie",
              "Station number": 6396,
              "Latitude": 52.22621233332533,
              "Longitude": 20.99614977836609,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Grójecka – Pl. Narutowicza",
              "Station number": 6397,
              "Latitude": 52.2179128907448,
              "Longitude": 20.9828272461891,
              "District": "Ochota"
          },
          {
              "Localization": "ul. Białobrzeska – Kopińska",
              "Station number": 6398,
              "Latitude": 52.21787180867202,
              "Longitude": 20.977578163146973,
              "District": "Ochota"
          },
          {
              "Localization": "Rondo Wiatraczna",
              "Station number": 6399,
              "Latitude": 52.24450144963282,
              "Longitude": 21.08378291130066,
              "District": "Praga Płd."
          },
          {
              "Localization": "ul. Książęca – Rozbrat",
              "Station number": 6400,
              "Latitude": 52.2297445891999,
              "Longitude": 21.030235290527344,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Świętokrzyska rejon Nowego Światu",
              "Station number": 6401,
              "Latitude": 52.23689695730212,
              "Longitude": 21.017639636993408,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Trębacka – Krakowskie Przedmieście",
              "Station number": 6402,
              "Latitude": 52.243575183764456,
              "Longitude": 21.01413130760193,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Warecka – Nowy Świat",
              "Station number": 6403,
              "Latitude": 52.23550731314584,
              "Longitude": 21.01809561252594,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Mokotowska – Nowy Świat",
              "Station number": 6404,
              "Latitude": 52.223747802336106,
              "Longitude": 21.020745635032654,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Wąwozowa – al. KEN- Metro Kabaty",
              "Station number": 6405,
              "Latitude": 52.13106776117692,
              "Longitude": 21.06556534767151,
              "District": "Ursynów"
          },
          {
              "Localization": "al. Komisji Edukacji Narodowej – Metro Natolin",
              "Station number": 6406,
              "Latitude": 52.1402575395535,
              "Longitude": 21.0569125413895,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Grzybowska – Żelazna",
              "Station number": 6407,
              "Latitude": 52.2345611475481,
              "Longitude": 20.990667343139645,
              "District": "Wola"
          },
          {
              "Localization": "ul. Wybrzeże Gdańskie – Sanguszki – Skwer 1 Dywizji Pancernej",
              "Station number": 6408,
              "Latitude": 52.2558745493,
              "Longitude": 21.0098960995674,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Wybrzeże Gdańskie – Sanguszki – Skwer 1 Dywizji Pancernej 1",
              "Station number": 64081,
              "Latitude": 52.2557858878398,
              "Longitude": 21.0102474689484,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Słowackiego – Metro Marymont",
              "Station number": 6409,
              "Latitude": 52.27137444520264,
              "Longitude": 20.97262144088745,
              "District": "Żoliborz"
          },
          {
              "Localization": "al. Jana Pawła II – Plac Grunwaldzki",
              "Station number": 6410,
              "Latitude": 52.2623069578896,
              "Longitude": 20.980281829834,
              "District": "Żoliborz"
          },
          {
              "Localization": "Plac Inwalidów",
              "Station number": 6411,
              "Latitude": 52.26442790492038,
              "Longitude": 20.989594459533688,
              "District": "Żoliborz"
          },
          {
              "Localization": "ul. Żelazna – ul. Sienna",
              "Station number": 6412,
              "Latitude": 52.2298176957136,
              "Longitude": 20.993977189064,
              "District": "Wola"
          },
          {
              "Localization": "al. Solidarności – Urząd Dzielnicy Wola",
              "Station number": 6413,
              "Latitude": 52.2401409335974,
              "Longitude": 20.9878054261208,
              "District": "Wola"
          },
          {
              "Localization": "ul. Matejki – Wiejska",
              "Station number": 6414,
              "Latitude": 52.2248880756772,
              "Longitude": 21.026930809021,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Chłodna – Żelazna",
              "Station number": 6415,
              "Latitude": 52.23725503822611,
              "Longitude": 20.98886489868164,
              "District": "Wola"
          },
          {
              "Localization": "al. Lotników – Modzelewskiego",
              "Station number": 6416,
              "Latitude": 52.1743988442479,
              "Longitude": 21.0128492116928,
              "District": "Mokotów"
          },
          {
              "Localization": "al. Niepodległości – Wilanowska",
              "Station number": 6417,
              "Latitude": 52.1800618456735,
              "Longitude": 21.0208381712437,
              "District": "Mokotów"
          },
          {
              "Localization": "pl. Powstańców Warszawy",
              "Station number": 6418,
              "Latitude": 52.23584240862528,
              "Longitude": 21.01359486579895,
              "District": "Śródmieście"
          },
          {
              "Localization": "Fabryka Wódek Koneser",
              "Station number": 6419,
              "Latitude": 52.254649691956814,
              "Longitude": 21.04408085346222,
              "District": "Praga Płn."
          },
          {
              "Localization": "al. Niepodległości – Batorego",
              "Station number": 6420,
              "Latitude": 52.2099077420935,
              "Longitude": 21.0068035125732,
              "District": "Mokotów"
          },
          {
              "Localization": "al. Niepodległości – Batorego 1",
              "Station number": 64201,
              "Latitude": 52.20995376226247,
              "Longitude": 21.00681960582733,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Marymoncka – Dewajtis",
              "Station number": 6421,
              "Latitude": 52.2901699484755,
              "Longitude": 20.9503228962421,
              "District": "Bielany"
          },
          {
              "Localization": "ul. Radzymińska – Piotra Skargi",
              "Station number": 6422,
              "Latitude": 52.26689674334066,
              "Longitude": 21.058607697486877,
              "District": "Targówek"
          },
          {
              "Localization": "ul. Stefana Banacha – Uniwersytet Warszawski",
              "Station number": 6423,
              "Latitude": 52.210670355885966,
              "Longitude": 20.98487913608551,
              "District": "Ochota"
          },
          {
              "Localization": "ul. Pasteura – Winnicka – UW",
              "Station number": 6424,
              "Latitude": 52.212984414159,
              "Longitude": 20.9820520877838,
              "District": "Ochota"
          },
          {
              "Localization": "ul. Stalowa – ul. 11-go Listopada",
              "Station number": 6425,
              "Latitude": 52.25823878953007,
              "Longitude": 21.035267114639282,
              "District": "Praga Płn."
          },
          {
              "Localization": "Metro Młociny",
              "Station number": 6426,
              "Latitude": 52.29079009373927,
              "Longitude": 20.929625630378723,
              "District": "Bielany"
          },
          {
              "Localization": "ul. Grójecka – Bitwy Warszawskiej 1920 roku",
              "Station number": 6427,
              "Latitude": 52.210768968782624,
              "Longitude": 20.9759259223938,
              "District": "Ochota"
          },
          {
              "Localization": "Port Czerniakowski",
              "Station number": 6428,
              "Latitude": 52.22769427210073,
              "Longitude": 21.044209599494934,
              "District": "Śródmieście"
          },
          {
              "Localization": "KS Spójnia",
              "Station number": 6429,
              "Latitude": 52.267658378510085,
              "Longitude": 21.002297401428223,
              "District": "Żoliborz"
          },
          {
              "Localization": "ul. Produkcyjna – ul. Modlińska",
              "Station number": 6430,
              "Latitude": 52.3210600127435,
              "Longitude": 20.971288383007,
              "District": "Białołęka"
          },
          {
              "Localization": "Hanki Ordonówny",
              "Station number": 6431,
              "Latitude": 52.332500346975,
              "Longitude": 20.9407916665077,
              "District": "Białołęka"
          },
          {
              "Localization": "Myśliborska – Trasa Mostu Marii Skłodowskiej-Curie",
              "Station number": 6432,
              "Latitude": 52.31321448607098,
              "Longitude": 20.965529680252075,
              "District": "Białołęka"
          },
          {
              "Localization": "Skarbka z Gór",
              "Station number": 6433,
              "Latitude": 52.3148509301452,
              "Longitude": 21.061123609542843,
              "District": "Białołęka"
          },
          {
              "Localization": "Plac Wilsona",
              "Station number": 6434,
              "Latitude": 52.269332616724064,
              "Longitude": 20.98337173461914,
              "District": "Żoliborz"
          },
          {
              "Localization": "Potocka",
              "Station number": 6435,
              "Latitude": 52.276271824059776,
              "Longitude": 20.98367214202881,
              "District": "Żoliborz"
          },
          {
              "Localization": "ul. Broniewskiego – ul. Włościańska",
              "Station number": 6436,
              "Latitude": 52.2672184701377,
              "Longitude": 20.9623968601227,
              "District": "Żoliborz"
          },
          {
              "Localization": "Poleczki – Business Park",
              "Station number": 6437,
              "Latitude": 52.1559985719703,
              "Longitude": 20.9962141513824,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Blokowa- DK Zacisze",
              "Station number": 6438,
              "Latitude": 52.2835315535238,
              "Longitude": 21.07213139534,
              "District": "Targówek"
          },
          {
              "Localization": "ul. Rembielińska – Kondratowicza",
              "Station number": 6439,
              "Latitude": 52.2939628704402,
              "Longitude": 21.0273116827011,
              "District": "Targówek"
          },
          {
              "Localization": "Rondo Żaba – ul. Św. Wincentego",
              "Station number": 6440,
              "Latitude": 52.26901418805055,
              "Longitude": 21.03793859481811,
              "District": "Targówek"
          },
          {
              "Localization": "ul. Kołowa – Teatr Rampa",
              "Station number": 6441,
              "Latitude": 52.27231654335543,
              "Longitude": 21.049869060516357,
              "District": "Targówek"
          },
          {
              "Localization": "ul. Kondratowicza – Urząd Dzielnicy Targówek",
              "Station number": 6442,
              "Latitude": 52.29162022169387,
              "Longitude": 21.048860549926758,
              "District": "Targówek"
          },
          {
              "Localization": "ul. Chodecka – ul. Żuromińska",
              "Station number": 6443,
              "Latitude": 52.2866359391383,
              "Longitude": 21.040256023407,
              "District": "Targówek"
          },
          {
              "Localization": "Galeria Mokotów",
              "Station number": 6444,
              "Latitude": 52.17855364649343,
              "Longitude": 21.002769470214844,
              "District": "Mokotów"
          },
          {
              "Localization": "Galeria Mokotów 1",
              "Station number": 64441,
              "Latitude": 52.18027400917562,
              "Longitude": 21.00259780883789,
              "District": "Mokotów"
          },
          {
              "Localization": "OSiR Włochy",
              "Station number": 6445,
              "Latitude": 52.1882400983785,
              "Longitude": 20.9603476524353,
              "District": "Włochy"
          },
          {
              "Localization": "ul. Wąwozowa – ul.Rosoła",
              "Station number": 6446,
              "Latitude": 52.1331653444223,
              "Longitude": 21.0748189687729,
              "District": "Ursynów"
          },
          {
              "Localization": "al. Jerozolimskie – rejon ulicy Popularnej",
              "Station number": 6447,
              "Latitude": 52.2011367425548,
              "Longitude": 20.9353065490723,
              "District": "Włochy"
          },
          {
              "Localization": "PKP Włochy",
              "Station number": 6448,
              "Latitude": 52.20575586928424,
              "Longitude": 20.916021466255184,
              "District": "Włochy"
          },
          {
              "Localization": "ul. Stryjeńskich – ul. Belgradzka",
              "Station number": 6449,
              "Latitude": 52.1375116703874,
              "Longitude": 21.0504108667374,
              "District": "Ursynów"
          },
          {
              "Localization": "al. Krakowska – UD Włochy",
              "Station number": 6450,
              "Latitude": 52.192347569343056,
              "Longitude": 20.9596449136734,
              "District": "Włochy"
          },
          {
              "Localization": "ul. Stryjeńskich – Wąwozowa",
              "Station number": 6451,
              "Latitude": 52.131459624889,
              "Longitude": 21.0561454296112,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Szczęśliwicka – Drawska",
              "Station number": 6452,
              "Latitude": 52.20833974071221,
              "Longitude": 20.96313714981079,
              "District": "Ochota"
          },
          {
              "Localization": "DK Włochy",
              "Station number": 6453,
              "Latitude": 52.1945869512993,
              "Longitude": 20.9668707847595,
              "District": "Włochy"
          },
          {
              "Localization": "Spektrum Tower",
              "Station number": 6454,
              "Latitude": 52.23438373925318,
              "Longitude": 20.99907875061035,
              "District": "Śródmieście"
          },
          {
              "Localization": "ul. Sanocka – Pruszkowska",
              "Station number": 6455,
              "Latitude": 52.20169895374808,
              "Longitude": 20.98214328289032,
              "District": "Ochota"
          },
          {
              "Localization": "ul. Belgradzka – ul. Rosoła",
              "Station number": 6456,
              "Latitude": 52.142088024955676,
              "Longitude": 21.065892577171322,
              "District": "Ursynów"
          },
          {
              "Localization": "ul. Szczęśliwicka – APS",
              "Station number": 6457,
              "Latitude": 52.2169154072792,
              "Longitude": 20.9710550308228,
              "District": "Ochota"
          },
          {
              "Localization": "ul. Cybernetyki",
              "Station number": 6458,
              "Latitude": 52.17563578556543,
              "Longitude": 20.99589228630066,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Jana Olbrachta – Górczewska",
              "Station number": 6459,
              "Latitude": 52.23903554451021,
              "Longitude": 20.94291865825653,
              "District": "Wola"
          },
          {
              "Localization": "ul. Leszno – ul. Młynarska",
              "Station number": 6460,
              "Latitude": 52.2367984024124,
              "Longitude": 20.97236394882202,
              "District": "Wola"
          },
          {
              "Localization": "Miasteczko Orange",
              "Station number": 6461,
              "Latitude": 52.208684902999,
              "Longitude": 20.9464645385742,
              "District": "Ochota"
          },
          {
              "Localization": "Miasteczko Orange 1",
              "Station number": 64611,
              "Latitude": 52.2082641333771,
              "Longitude": 20.9456920623779,
              "District": "Ochota"
          },
          {
              "Localization": "ul. Wolska – ul. Płocka",
              "Station number": 6462,
              "Latitude": 52.23332256001735,
              "Longitude": 20.965754985809323,
              "District": "Wola"
          },
          {
              "Localization": "Budynek LOT",
              "Station number": 6463,
              "Latitude": 52.1827080606605,
              "Longitude": 20.9695851802826,
              "District": "Włochy"
          },
          {
              "Localization": "Blue City",
              "Station number": 6464,
              "Latitude": 52.213530041019155,
              "Longitude": 20.95563769340515,
              "District": "Ochota"
          },
          {
              "Localization": "T-Mobile",
              "Station number": 6465,
              "Latitude": 52.18087924523222,
              "Longitude": 20.992426872253418,
              "District": "Mokotów"
          },
          {
              "Localization": "Mariensztat",
              "Station number": 6466,
              "Latitude": 52.24655099048534,
              "Longitude": 21.017478704452515,
              "District": "Śródmieście"
          },
          {
              "Localization": "EuroCentrum",
              "Station number": 6467,
              "Latitude": 52.22062915307417,
              "Longitude": 20.972299575805664,
              "District": "Ochota"
          },
          {
              "Localization": "Metro Wawrzyszew",
              "Station number": 6473,
              "Latitude": 52.28584837775539,
              "Longitude": 20.940569043159485,
              "District": "Bielany"
          },
          {
              "Localization": "ul. Grochowska – Urząd Dzielnicy Praga – Południe",
              "Station number": 6474,
              "Latitude": 52.24635392336338,
              "Longitude": 21.068853735923767,
              "District": "Praga Płd."
          },
          {
              "Localization": "ul. Afrykańska – Egipska",
              "Station number": 6475,
              "Latitude": 52.224996515133036,
              "Longitude": 21.0712730884552,
              "District": "Praga Płd."
          },
          {
              "Localization": "ul. Gen. R. Abrahama",
              "Station number": 6476,
              "Latitude": 52.228289005239326,
              "Longitude": 21.085622906684872,
              "District": "Praga Płd."
          },
          {
              "Localization": "ul. Rechniewskiego – Nowaka – Jeziorańskiego",
              "Station number": 6477,
              "Latitude": 52.22966573260081,
              "Longitude": 21.106356382369995,
              "District": "Praga Płd."
          },
          {
              "Localization": "pl. Szembeka",
              "Station number": 6478,
              "Latitude": 52.24263904435865,
              "Longitude": 21.102038025856018,
              "District": "Praga Płd."
          },
          {
              "Localization": "ul. Redutowa",
              "Station number": 6479,
              "Latitude": 52.22675779769572,
              "Longitude": 20.942344665527344,
              "District": "Wola"
          },
          {
              "Localization": "ul. Puławska – Dworkowa",
              "Station number": 6480,
              "Latitude": 52.20555533353896,
              "Longitude": 21.02306306362152,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Jana III Sobieskiego – Chełmska",
              "Station number": 6481,
              "Latitude": 52.201600320719834,
              "Longitude": 21.035878658294674,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Czerniakowska – ZUS",
              "Station number": 6482,
              "Latitude": 52.19951911278353,
              "Longitude": 21.05130672454834,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Św. Bonifacego – Pętla Stegny",
              "Station number": 6483,
              "Latitude": 52.177892456900445,
              "Longitude": 21.049649119377136,
              "District": "Mokotów"
          },
          {
              "Localization": "Metro Wierzbno",
              "Station number": 6484,
              "Latitude": 52.18887153573278,
              "Longitude": 21.01761817932129,
              "District": "Mokotów"
          },
          {
              "Localization": "Metro Racławicka",
              "Station number": 6485,
              "Latitude": 52.199288957421864,
              "Longitude": 21.012194752693176,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Jana III Sobieskiego – rejon ul. Św. Bonifacego",
              "Station number": 6486,
              "Latitude": 52.18187918241075,
              "Longitude": 21.056467294692993,
              "District": "Mokotów"
          },
          {
              "Localization": "ul. Czerniakowska – rejon ul. J. Gagarina",
              "Station number": 6487,
              "Latitude": 52.207307525111965,
              "Longitude": 21.04801297187805,
              "District": "Mokotów"
          },
          {
              "Localization": "UD Ursus",
              "Station number": 6488,
              "Latitude": 52.2005646607065,
              "Longitude": 20.893737673759457,
              "District": "Ursus"
          },
          {
              "Localization": "PKP Ursus",
              "Station number": 6489,
              "Latitude": 52.19596472349465,
              "Longitude": 20.884934663772583,
              "District": "Ursus"
          },
          {
              "Localization": "OSiR Ursus",
              "Station number": 6490,
              "Latitude": 52.18691799737384,
              "Longitude": 20.89069604873657,
              "District": "Ursus"
          },
          {
              "Localization": "ul. Skoroszewska – Prystora",
              "Station number": 6491,
              "Latitude": 52.19063753946811,
              "Longitude": 20.900035500526428,
              "District": "Ursus"
          },
          {
              "Localization": "PKP Ursus Niedźwiadek",
              "Station number": 6492,
              "Latitude": 52.19164712275148,
              "Longitude": 20.868959426879883,
              "District": "Ursus"
          },
          {
              "Localization": "ul. Jana III Sobieskiego – Wilanowska",
              "Station number": 6493,
              "Latitude": 52.17031274575778,
              "Longitude": 21.068408489227295,
              "District": "Wilanów"
          },
          {
              "Localization": "ul. Przyczółkowa – Branickiego",
              "Station number": 6494,
              "Latitude": 52.15923035794532,
              "Longitude": 21.08649730682373,
              "District": "Wilanów"
          },
          {
              "Localization": "Wola Park",
              "Station number": 6495,
              "Latitude": 52.241344840165176,
              "Longitude": 20.92921257019043,
              "District": "Wola"
          },
          {
              "Localization": "Atrium Targówek",
              "Station number": 6496,
              "Latitude": 52.30273849910207,
              "Longitude": 21.058843731880188,
              "District": "Targówek"
          },
          {
              "Localization": "al. Rzeczypospolitej / ul. Branickiego",
              "Station number": 6497,
              "Latitude": 52.156778566238266,
              "Longitude": 21.076213717460632,
              "District": "Wilanów"
          },
          {
              "Localization": "Atrium Promenada",
              "Station number": 6498,
              "Latitude": 52.231834238039944,
              "Longitude": 21.104929447174072,
              "District": "Praga Płd."
          },
          {
              "Localization": "Rondo Vogla – Syta",
              "Station number": 6499,
              "Latitude": 52.164775187347715,
              "Longitude": 21.10926389694214,
              "District": "Wilanów"
          }
        ]
    }
}

