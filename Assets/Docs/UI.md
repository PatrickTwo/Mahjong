## 缁撹锛氫綘鐨勬€濊矾鏄悎鐞嗙殑锛圥age/Panel + UIManager + 璇锋眰閫昏緫 + 浜嬩欢椹卞姩锛夛紝浣嗚閬垮厤涓ょ被甯歌鈥滃悗鏈熷繀鐥涒€濈殑鑰﹀悎

- **鏁翠綋鍚堢悊鎬?*锛? 
  浣犳弿杩扮殑閾捐矾銆宍UI(Page/Panel)` 鈫?`UIRequestHandler`锛堣姹?鍛戒护锛夆啋 閫昏緫灞傦紙鐢ㄤ緥/鏈嶅姟锛夆啋锛堜簨浠?鐘舵€佸彉鏇达級鈫?UI鍒锋柊銆嶅湪 Unity 椤圭洰閲屾槸闈炲父甯歌涓斿彲鎵╁睍鐨勩€? 
  浣犻」鐩噷涔熷凡缁忔湁瀵瑰簲鍩虹锛歚GameFlowController` 鐘舵€佹満鍜?`MahjongGameManager.OnStateChanged`锛堥€昏緫渚т簨浠讹級銆傝锛?

```21:57:C:/Users/ugion017/Desktop/Mahjong/Assets/Game/Scripts/Core/GameControl/MahjongGameManager.cs
public event Action<GameState> OnStateChanged;
public void TriggerStateChanged(GameState newState)
{
    CurrentState = newState;
    OnStateChanged?.Invoke(newState);
}
```

- **闇€瑕佽鎯曠殑鐐?*锛堝缓璁綘鍦ㄦ灦鏋勪笂鎻愬墠鈥滃畾瑙勭煩鈥濓級锛? 
  1) **浜嬩欢绯荤粺鐨勭被鍨嬪畨鍏ㄤ笌鐢熷懡鍛ㄦ湡**锛氫綘褰撳墠鐨?`TypeEventSystem` 鑷姩娉ㄩ攢渚濊禆鍙嶅皠锛屽鏌ユ枃妗ｅ凡缁忔寚鍑?*鍙兘宕╂簝**鍜屽皝瑁呮€ч棶棰樸€? 
  2) **UIManager 鍙樻垚鈥滀笂甯濈被鈥?*锛氬鏋?UIManager 鍚屾椂绠¤祫婧愬姞杞姐€佸鑸爤銆佸姩鐢汇€侀伄缃┿€佽緭鍏ュ睆钄姐€佹暟鎹粦瀹氥€佸脊绐楅槦鍒楋紝寰堝揩浼氳噧鑲夸笖闅炬祴銆?

---

## 鎺ㄨ崘鐨勬洿绋宠璁★紙鍦ㄤ綘鐜版湁妗嗘灦涓婃渶瀹规槗钀藉湴锛?

### 1) 鐢ㄢ€滃鑸?灞曠ず鈥濅笌鈥滀笟鍔′氦浜掆€濆垎灞傦細UIManager 鍙仛瀵艰埅涓庣敓鍛藉懆鏈?
- **UIManager 璐熻矗**锛?
  - Page 鐨勬墦寮€/鍏抽棴锛堝缓璁仛鎴?*鏍?*锛歅ush/Pop/Replace锛?
  - Panel 鐨勬樉绀洪殣钘忥紙寤鸿鍒嗗眰锛欻UD灞傘€丳opup灞傘€乀oast灞傜瓑锛?
  - UI 鐨勭敓鍛藉懆鏈燂紙Instantiate/Show/Hide/Dispose锛夛紝鍙€夊姞 Addressables
- **UIManager 涓嶈礋璐?*锛?
  - 涓氬姟閫昏緫銆侀夯灏嗚鍒欍€佺綉缁滃崗璁€佽鍒嗙粏鑺?

杩欐牱浣犵殑 Page/Panel 鏋舵瀯浼氬緢娓呯埥锛?*UIManager = 瀵艰埅绯荤粺**锛岃€屼笉鏄笟鍔″叆鍙ｃ€?

### 2) `UIRequestHandler` 鏇撮€傚悎浣滀负鈥滅敤渚嬪叆鍙ｏ紙Application Layer锛夆€濊€屼笉鏄€滈殢澶勫彲鍐欑殑宸ュ叿绫烩€?
浣犵幇鍦ㄧ殑 `UIRequestHandler.cs` 杩樻槸绌虹殑锛?

```1:18:C:/Users/ugion017/Desktop/Mahjong/Assets/Game/Scripts/Core/UI/UIRequestHandler.cs
public class UIRequestHandler : MonoBehaviour
{
    void Start() { }
    void Update() { }
}
```

寤鸿浣犵粰瀹冧竴涓槑纭畾浣嶏細  
- **鍙毚闇测€滅敤鎴锋剰鍥锯€濈殑鏂规硶**锛堝懡浠ゅ紡锛夛細濡?`CreateRoom()`銆乣Ready()`銆乣StartGame()`銆乣Discard(tileId)`  
- **鍐呴儴鍐嶈皟鐢ㄩ€昏緫灞?*锛圡anager/Service/UseCase锛? 
- **涓嶈 UI 鐩存帴鎿嶄綔 GameManager 鐨勫唴閮ㄥ璞?*锛堥伩鍏?UI 鍜屾牳蹇冨璞″己鑰﹀悎锛?

杩欏湪妯″紡涓婃洿鎺ヨ繎 **MVP/MVVM 涓殑 Presenter/ViewModel** 鎴?**Clean Architecture 鐨?UseCase**銆?

### 3) 鈥滈€昏緫灞傗啋浜嬩欢鈫扷I鈥濇槸瀵圭殑锛屼絾浜嬩欢鏈€濂戒紶鈥滅姸鎬佸揩鐓?DTO鈥濓紝鍒紶鈥滃彲鍙樺紩鐢ㄢ€?
浣犳湁涓ゆ潯鍙璺嚎锛?

- **璺嚎A锛堟洿绠€鍗曪級**锛歎I 璁㈤槄 `MahjongGameManager` 鐨勪簨浠讹紙濡?`OnStateChanged`銆乣OnPlayerAction`锛夛紝UI鏀跺埌鍚庡仛娓叉煋銆? 
  浼樼偣锛氱洿瑙傘€佸皯涓€濂楃郴缁熴€傜己鐐癸細濡傛灉浜嬩欢寰堝锛孧anager 浼氳啫鑳€銆?

- **璺嚎B锛堟洿瑙勮寖锛?*锛氶€昏緫灞傛妸鍙樺寲鍐欏叆涓€涓彧璇?`GameModel` / `GameStore`锛堢姸鎬佷粨搴擄級锛屽啀鍙?`GameStateChanged` / `HandChanged` 绛変簨浠讹紱UI璇讳粨搴撳埛鏂般€? 
  浼樼偣锛歎I 涓嶉渶瑕佷粠浜嬩欢鍙傛暟鎷垮埌鎵€鏈夋暟鎹紱閲嶆斁/鏂嚎閲嶈繛/鍥炴斁鏇村鏄撱€傜己鐐癸細澶氫竴涓?Store 姒傚康銆?

鏃犺鍝潯璺嚎锛岄兘寤鸿浜嬩欢鍙傛暟浣跨敤**涓嶅彲鍙樻暟鎹?*锛坰truct/record/鍙 DTO锛夛紝閬垮厤 UI 鎷垮埌瀵硅薄寮曠敤鍚庤鏀规牳蹇冪姸鎬併€?

---

## 浣犲綋鍓嶄簨浠剁郴缁熺殑寤鸿锛堥噸瑕侊級
浣犻」鐩噷鑷冲皯鏈変袱濂椾簨浠朵綋绯伙細鑷爺鐨?`TypeEventSystem`锛屼互鍙?JKFrame 鐨?`EventModule`锛堝瓧绗︿覆浜嬩欢鍚嶏級銆傚悓鏃跺瓨鍦ㄤ細瀵艰嚧鍥㈤槦浣跨敤娣蜂贡銆?

### 寤鸿
- **灏介噺鍙繚鐣欎竴濂椾簨浠舵満鍒?*锛堝苟涓斾繚璇侊細绫诲瀷瀹夊叏銆佸彲鑷姩瑙ｇ粦銆佷笉闈犲弽灏勬垨鎶婂弽灏勯殧绂诲湪寰堝皯璺緞涓婏級
- 濡傛灉缁х画鐢?`TypeEventSystem`锛氳嚦灏戣瑙ｅ喅瀹℃煡閲屾寚鍑虹殑鍏抽敭闂锛堝弽灏勭Щ闄ゃ€乣event` 鍏抽敭瀛椼€乶ull/default 琛屼负绛夛級銆備綘宸叉湁瀹℃煡鎶ュ憡宸茬粡鎶婇闄╄寰楀緢娓呮锛坄TypeEventSystem_Review.md`锛夈€?

---

## 鏇粹€滄ā寮忓寲鈥濈殑瀵圭収琛紙浣犵幇鍦ㄧ殑鎻忚堪瀵瑰簲浠€涔堢粡鍏告ā寮忥級
- **Page/Panel + UIManager**锛歎I Navigation / UI Composition Root锛堝父瑙佷簬鎵嬫父锛?
- **UIRequestHandler**锛歅resenter / ViewModel / UseCase Facade锛堝彇鍐充簬浣犳槸鍚﹀仛鏁版嵁缁戝畾锛?
- **浜嬩欢椹卞姩鏇存柊UI**锛歄bserver +锛堝彲閫夛級Redux/Store 椋庢牸

---

## 鏈€鎺ㄨ崘鐨勨€滀緷璧栨柟鍚戔€濅竴鏉＄嚎锛堥伩鍏嶈€﹀悎锛?
- **UI锛圴iew锛?* 鈫?鍙緷璧?`UIRequestHandler`锛堟帴鍙?闂ㄩ潰锛? 
- **UIRequestHandler锛堢敤渚嬪眰锛?* 鈫?渚濊禆鏍稿績閫昏緫锛圡anager/Service锛? 
- **鏍稿績閫昏緫** 鈫?鍙彂甯冧簨浠?鏇存柊 Store锛屼笉渚濊禆浠讳綍 UI 绫? 
- **UI** 鈫?璁㈤槄浜嬩欢/璇诲彇 Store锛堟覆鏌擄級

杩欐牱浣犲皢鏉ユ崲 UI 妗嗘灦銆佹崲鍦烘櫙缁勭粐銆佺敋鑷冲仛鏃犲ご娴嬭瘯锛圚eadless锛夐兘浼氭洿瀹规槗銆?

---

## 浣犲彲浠ョ珛鍒诲仛鐨勪竴涓皬楠屾敹鏍囧噯锛堝垽鏂灦鏋勬槸鍚︹€滃仴搴封€濓級
- **鍒犻櫎鏌愪釜 Page 鐨?prefab/鑴氭湰鏃?*锛屾牳蹇冮€昏緫搴斿綋杩樿兘璺戯紙鏈€澶氱己 UI锛夈€? 
- **鏍稿績閫昏緫绋嬪簭闆嗭紙鎴栫洰褰曪級**閲屼笉搴旇鍑虹幇 `using UnityEngine.UI`銆佷篃涓嶅簲璇ュ紩鐢?`LobbyUI/GameUI`銆? 
- UI 鑴氭湰閲屼笉搴旇鍑虹幇澶ч噺 鈥滃埌澶勬嬁鍗曚緥鈥?鏀圭姸鎬侊紙渚嬪鐩存帴鏀?`MahjongGameManager` 鍐呴儴闆嗗悎锛夛紝搴旇缁熶竴璧拌姹傚叆鍙ｃ€?

---

濡傛灉浣犳効鎰忥紝鎴戝彲浠ョ户缁府浣犳妸鈥滀綘璁炬兂鐨?UIManager/Page/Panel + RequestHandler + 浜嬩欢/Store鈥濈殑绫昏亴璐ｅ垝鍒嗘垚涓€寮犳洿鍏蜂綋鐨勬竻鍗曪紙姣忎釜绫昏鏈夊摢浜涙柟娉曘€佸摢浜涗簨浠躲€佸摢浜涗緷璧栵級锛屽苟瀵圭収浣犵幇鍦ㄧ洰褰曠粨鏋?`Assets/Game/Scripts/Core/UI/` 鍜?`GameControl/States/` 缁欏嚭鏈€璐村悎浣犻」鐩殑钀藉湴鏂规銆