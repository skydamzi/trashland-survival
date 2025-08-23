# Trashland Survival: 게임 아키텍처

## 1. 모듈화 (Modularity)
이 게임은 독립적인 모듈로 구성되어 시스템의 유연성과 확장성을 높입니다. 각 모듈은 명확하게 정의된 역할을 수행하며, 다른 모듈과의 상호작용은 최소화됩니다.

- **주요 모듈:**
  - `Player`: 플레이어의 움직임, 상태, 입력을 관리합니다.
  - `Weapon`: 무기의 발사, 재장전, 데미지 계산 등 무기 관련 로직을 처리합니다.
  - `Equipment`: 플레이어가 장착하는 장비의 효과와 로직을 담당합니다.
  - `GameLogic`: 게임의 핵심 규칙, 상태(시작, 종료, 일시정지), 승리/패배 조건 등을 관리합니다.
  - `UI`: 사용자 인터페이스(HUD, 메뉴, 설정)를 표시하고 사용자 입력을 처리합니다.
  - `Monster`: 몬스터의 AI, 이동, 공격, 상태를 관리합니다.
  - `Wave`: 몬스터 웨이브의 생성, 타이밍, 구성을 관리합니다.

## 2. 설계 원칙
### 관심사 분리 (Separation of Concerns, SoC)
각 클래스는 하나의 명확한 책임만 가집니다. 예를 들어, `PlayerController`는 플레이어의 입력을 받아 움직임을 처리하고, `PlayerHealth`는 플레이어의 체력과 데미지 처리만 담당합니다. 이를 통해 코드의 이해와 테스트가 용이해집니다.

### 느슨한 결합 (Loose Coupling)
모듈 간의 직접적인 참조를 최소화하여 의존성을 줄입니다.
- **이벤트 기반 통신**: `UnityEvent` 또는 자체 구현한 `EventBus`를 사용하여 모듈 간의 통신을 중재합니다. 예를 들어, 몬스터가 죽었을 때 `Monster` 모듈은 `OnMonsterDied` 이벤트를 발생시키고, `GameLogic`이나 `UI` 모듈은 이 이벤트를 구독하여 점수를 업데이트하거나 처치 수를 표시합니다.

### 확장성 (Extensibility)
새로운 기능을 쉽게 추가하고 기존 기능을 수정할 수 있도록 설계합니다.
- **ScriptableObject 활용**: 무기, 장비, 몬스터, 웨이브와 같은 게임의 핵심 데이터를 `ScriptableObject`로 정의합니다. 이를 통해 개발자가 코드를 직접 수정하지 않고도 새로운 아이템이나 몬스터를 추가하고, 기존 데이터를 쉽게 수정할 수 있습니다.
- **인터페이스 기반 설계**: 공통 기능을 인터페이스로 추상화하여 다양한 구현을 가능하게 합니다.
  - `IWeapon`: 모든 무기 클래스가 구현해야 하는 인터페이스입니다.
  - `IEquipment`: 모든 장비 클래스가 구현해야 하는 인터페이스입니다.
  - `IMonster`: 모든 몬스터 클래스가 구현해야 하는 인터페이스입니다.
  - `IDamageable`: 데미지를 받을 수 있는 모든 객체(플레이어, 몬스터 등)가 구현합니다.
  - `IDamageDealer`: 데미지를 줄 수 있는 모든 객체(무기, 몬스터 스킬 등)가 구현합니다.

## 3. 재사용성 (Reusability)
코드와 데이터의 재사용성을 극대화하여 개발 효율성을 높입니다.
- **ScriptableObject 데이터**: 무기 스펙(데미지, 연사 속도), 몬스터 정보(체력, 공격력), 웨이브 구성 등은 `ScriptableObject`를 통해 데이터화되어 여러 곳에서 재사용됩니다.
- **기반 클래스**: 공통 로직은 기반 클래스(`BaseMonster`, `BaseWeapon`)로 구현하여, 이를 상속하는 하위 클래스에서 코드를 중복 작성할 필요가 없도록 합니다.

## 4. 유지보수성 (Maintainability)
코드의 가독성과 일관성을 유지하여 버그 수정 및 기능 개선이 용이하도록 합니다.
- **코딩 컨벤션**: 명확하고 일관된 코딩 스타일(네이밍, форматирование)을 따릅니다.
- **DRY (Don't Repeat Yourself)**: 중복 코드를 최소화하고, 공통 로직은 함수나 클래스로 분리합니다.
- **KISS (Keep It Simple, Stupid)**: 복잡한 문제를 단순하게 해결하고, 불필요한 복잡성을 피합니다.
- **기획자를 위한 데이터 설계**: 게임의 핵심 데이터는 `ScriptableObject` 기반으로 설계되어, 기획자가 Unity 에디터 내에서 직접 게임 밸런스를 수정하고 새로운 콘텐츠를 테스트할 수 있습니다.

## 5. 구현된 스크립트 목록

### Core (핵심)
- **`IDamageable.cs`**: 데미지를 받을 수 있는 객체를 위한 인터페이스입니다. `TakeDamage(float damage)` 메서드를 정의합니다.
- **`IDamageDealer.cs`**: 데미지를 주는 객체를 위한 인터페이스입니다. `GetDamage()`와 `GetOwner()` 메서드를 정의합니다.

### Manager (관리)
- **`GameManager.cs`**: 게임의 전반적인 상태(`Ready`, `Playing`, `Paused`, `GameOver`)를 관리하는 싱글톤 클래스입니다. 씬 전환, 시간 흐름 제어, 게임 시작/종료 로직을 담당합니다.
- **`PlayerManager.cs`**: 플레이어의 핵심 데이터(레벨, 체력, 경험치, 능력치)를 관리하는 싱글톤 클래스입니다. 플레이어 데이터 초기화, 레벨업, 피격 처리 및 무적 효과를 담당합니다.
- **`PoolManager.cs`**: 오브젝트 풀링을 관리하는 싱글톤 클래스입니다. 게임 오브젝트를 미리 생성하고 재사용하여 성능을 최적화합니다.
- **`UIManager.cs`**: UI를 관리하고 게임 상태에 따라 UI 패널(게임 오버, 일시정지 등)을 활성화/비활성화합니다. 플레이어의 체력, 경험치 등 정보를 텍스트로 표시합니다.
- **`SoundManager.cs`**: 배경음악(BGM)과 효과음(SFX)을 재생하는 싱글톤 클래스입니다.
- **`GameEvents.cs`**: 게임의 주요 이벤트를 정의하는 정적 클래스입니다. `OnGameOver`, `OnGamePaused` 등의 이벤트를 통해 모듈 간의 느슨한 결합을 구현합니다.

### GameLogic (게임 로직)
- **`MonsterSpawner.cs`**: `WaveData` ScriptableObject를 기반으로 몬스터 웨이브를 생성합니다. 화면 밖 랜덤한 위치에 몬스터를 스폰합니다.
- **`WaveData.cs`**: `ScriptableObject`를 사용하여 각 웨이브의 지속 시간, 몬스터 종류 및 수를 정의합니다.
- **`Poolable.cs`**: 풀링할 게임 오브젝트에 추가하는 컴포넌트로, 원본 프리팹 정보를 저장합니다.
- **`FollowCamera.cs`**: 플레이어를 부드럽게 따라다니는 카메라 로직을 구현합니다.
- **`EXPGem.cs`**: 몬스터가 드랍하는 경험치 보석입니다. 플레이어에게 다가가면 경험치를 제공하고 풀로 반환됩니다.
- **`FindPlayer.cs`**: 게임 시작 시 플레이어 오브젝트를 찾아 `PlayerManager`에 등록합니다.

### Player (플레이어)
- **`Player.cs`**: 가상 조이스틱 입력을 받아 플레이어의 이동과 애니메이션을 처리합니다.
- **`PlayerAttackController.cs`**: 가장 가까운 몬스터를 탐지하고 현재 장착된 무기로 공격을 수행합니다.
- **`PlayerDamageable.cs`**: `IDamageable` 인터페이스를 구현하여, 피격 시 `PlayerManager`에 데미지를 전달합니다.
- **`WeaponBase.cs`**: 모든 무기의 기반이 되는 추상 클래스입니다. `Attack(Transform target)` 추상 메서드를 정의합니다.
- **`Gun.cs`**: `WeaponBase`를 상속받은 원거리 무기입니다. `Bullet` 프리팹을 발사합니다.
- **`Bullet.cs`**: `Gun`에 의해 발사되는 총알입니다. 몬스터에게 닿으면 데미지를 주고 풀로 반환됩니다.
- **`Punch.cs`**: `WeaponBase`를 상속받은 근접 무기입니다. 목표를 향해 목이 늘어나는 공격을 합니다.
- **`Boomerang.cs`**: `WeaponBase`를 상속받은 투사체 무기입니다. `BoomerangProjectile`을 던지고 되돌아오는 로직을 관리합니다.
- **`BoomerangProjectile.cs`**: `Boomerang`에 의해 발사되는 투사체입니다. 적을 관통하며 원을 그리고 플레이어에게 돌아옵니다.

### Monster (몬스터)
- **`Monster.cs`**: `MonsterData`를 기반으로 몬스터의 능력치를 설정하고, 플레이어를 향해 이동합니다. `IDamageable`을 구현하여 데미지를 받으면 체력이 감소하고, 죽으면 경험치 보석을 드랍하고 풀로 반환됩니다.
- **`MonsterData.cs`**: `ScriptableObject`를 사용하여 몬스터의 이름, 프리팹, 체력, 공격력, 이동 속도, 드랍할 경험치 등을 정의합니다.
- **`MonsterHitbox.cs`**: 몬스터의 공격 판정을 담당하는 컴포넌트입니다. `IDamageDealer`를 구현하며, 플레이어에게 닿으면 지속적으로 데미지를 줍니다.

### UI (Buttons)
- **`StartButton.cs`**, **`PauseButton.cs`**, **`ResumeButton.cs`**, **`ExitButton.cs`**: 각 버튼의 `onClick` 이벤트에 `GameManager`의 해당 기능을 연결합니다.
- **`WeaponSelectButton.cs`**: 무기 선택 버튼에 사용되며, 클릭 시 `PlayerManager`에 선택된 무기 타입을 저장합니다.