# Gojyuon_KeyHook
## 2022/09/26 hhiro-GSX1100  
## Special Thanks to AonaSuzutsuki:https://aonasuzutsuki.hatenablog.jp/entry/2018/10/15/170958  

対応するIMEはMicrosoft IMEです。  

Windows10及びWindows11の現時点（2022/9/26)の最新の日本語IMEでは、カナ入力モードとローマ
字入力モードにおけるConversionModeの値が同値であるというバグがあり、この場合、カナ入力
ローマ字入力でもキーフックが発生してしまいます。  
また、カナ入力において最初に入力する文字が違う文字になるという障害もあります。

従って、MicrosoftIMEを用いる場合でも「以前のバージョンのMicrosoft IMEを使う」を有効にし
て“以前のバージョン”で使用してください。  

本アプリはキーボードの入力をフックして、他のキーを入力するようにしているだけであるため、
現状ではキーの置き換えのみが可能です。また、設定値を外部から読み込むようには作成していません。

## exeファイル
一応サンプル的にexeファイル「Gojyuon_KeyHook.exe」もUploadしていますが、Defenderがウイルス
判定しますので許可する必要があります。（他のアンチウイルスソフトウェアも同じと思われる）  
SHA256: 0a68dc4a2aabada08b03d50892852fd8aec35596d7b37a910a2ee93ef691bc64

## ■有効の場合のキー置換マップ  
【通常】  
の | ね | ぬ | に | な | あ | い | う | え | お | や | ゆ | よ  
ほ | へ | ふ | ひ | は | か | き | く | け | こ | ゛ | ゜ |   
も | め | む | み | ま | さ | し | す | せ | そ | ん | ー |   
ろ | れ | る | り | ら | た | ち | つ | て | と | わ |   

【Shiftを押した場合】  
の | 、 | ぬ | に | な | ぁ | ぃ | ぅ | ぇ | ぉ | ゃ | ゅ | ょ  
ほ | へ | ふ | ひ | は | か | き | く | け | こ | ・ | 「 |   
も | ・ | 」 | み | ま | さ | し | す | せ | そ | ん | 」 |   
ろ | れ | 。 | り | ら | た | ち | っ | 、 | 。 | を |  

## 謝辞
作成にあたっては、AonaSuzutsukiさんの記事  
「C# - グローバルキーフックでキーの捕捉と入力を行う」  
https://aonasuzutsuki.hatenablog.jp/entry/2018/10/15/170958  
を参考とさせていただきました。  
有用な記事の公開に対し、厚く御礼申し上げます。  
 
  
