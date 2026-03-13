# 大厅页流程设计
## 一、GameState: LobbyWaiting
Enter():
1. 加载LobbyScene

---

## 二、PageUI: LobbyPageUI
1. 左上角：开始游戏按钮(StartGameBtn)、游戏玩法设置(PlaySettingBtn)
2. 右上角：游戏设置(GameSettingBtn)、房间号(RoomID)、加入房间(JoinRoomBtn)
3. 中央：玩家列表（四名）
4. 右下角：麦克风(MicTog)、扬声器(SpeakerTog)
5. 左下角：聊天框(ChatBox)

#### 开始按钮(StartGameBtn):
1. 验证玩家人数是否足够（四名玩家才可开始），不足则弹框提醒
2. 验证玩家是否都已准备好，未准备好则弹框提醒
3. 如果所有玩家都准备好，开始游戏，进入选庄状态(GameState.SelectBanker)

#### 玩家列表：
1. 四个占位框，每个占位框包含换位、邀请、添加人机三个按钮
2. 每进入一名玩家，在对应占位框显示玩家信息（头像、昵称、准备状态），下方显示一个菜单键用于展开对玩家的操作（踢出房间）
3. 玩家进入后，默认显示为未准备状态

#### 麦克风(MicTog)与扬声器(SpeakerTog):
1. 点击麦克风图标，切换麦克风状态（开/关）
2. 点击扬声器图标，切换扬声器状态（开/关）
3. 麦克风默认关闭，扬声器默认开启
4. 只有在开启扬声器的情况下，才能开启麦克风
---
## 三、PanelUI: 
1. 进房面板JoinRoomPanelUI; 
2. 对局设置面板PlaySettingPanelUI; 
3. 游戏设置面板GameSettingPanelUI; 
4. 聊天框面板ChatBoxPanelUI; 
5. 玩家操作面板PlayerOperationPanelUI;  
6. 玩家信息面板PlayerInfoPanelUI;
7. 提示框面板PromptPanelUI;

#### 1.进房面板JoinRoomPanelUI
1. 包含房间号输入框、加入房间按钮
2. 玩家输入房间号后，点击加入房间按钮，加入对应的房间
3. 如果房间不存在或已满，弹框提醒玩家

#### 2.对局设置面板PlaySettingPanelUI
分为玩法设置、游戏规则设置、计分设置
玩法设置：
1. 选择不同玩法（川麻、大众麻将、红中麻将等）
2. 选择不同玩法的具体规则（如是否可以吃牌等）
游戏规则设置：
1. 出牌回合时长：每个玩家出牌的时间限制，超过时间未出牌则自动跳过该玩家
计分设置：
1. 底分：用于每回合的结算
2. 玩家初始分数

#### 3.游戏设置面板GameSettingPanelUI
1. 音量(VolumeSlider)：玩家可以通过调整音量滑块，改变游戏内的音量大小

#### 4.聊天框面板ChatBoxPanelUI
1. 分为两个区域，上方显示聊天记录，下方显示输入框和发送按钮
2. 玩家输入聊天内容后，点击发送按钮，将聊天内容发送到聊天记录区域
3. 聊天内容显示在聊天框中，包括玩家昵称和消息内容

#### 5.玩家操作面板PlayerOperationPanelUI
1. 踢出玩家

#### 6.玩家信息面板PlayerInfoPanelUI
1. 点击玩家头像显示该玩家的详细信息（头像、昵称、准备状态、分数等）

#### 7.提示框面板PromptPanelUI
1. 用于显示提示信息，如提示玩家准备等








