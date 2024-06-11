# TINY Z (The City)

![TINY Z Logo](https://github.com/SnowScapes/ApocalypseSurvival/assets/167047045/37ca669a-8c6d-4e6e-9c84-616c22c12055)

<br><br>

## 🧟: 목차 
| [🏴 프로젝트 소개 ☢️](#-프로젝트-소개) |
| :---: |
| [ 사용 기술 스택 ☢️](#-사용-기술-스택) |
| [☢️ 기술적 고민과 트러블 슈팅 🍵](#-기술적-고민과-트러블-슈팅) |
| [🏴 만든 사람들🏴](#-만든-사람들) |

<br><br>

* * *

<br><br>

# 🧟: 프로젝트 소개

### [

### [

<br><br>

### ⚒️좀비들이 가득한 세상 속에서 홀로 남은 당신은 최대한 오래 이 도시에서 살아남아야 합니다!⚒️
![게임 미리보기]

| 게임명 | TINY Z (The City) |
| :---: | :---: |
| 장르 | 생존 서바이벌 |
| 개발 환경 | Unity 2022.3.17f1 |
| 타겟 플랫폼 | PC |
| 개발 기간 | 2024.06.03 ~ 2024.06.11 |

<br><br>

[☢️ 목차로 돌아가기](#-목차)

<br><br>

---

<br><br>

# 🧟: 사용 기술 스택
#### 클릭하면 자세한 내용을 확인하실 수 있습니다!⚒️

1. Unity 2022.3.17f1
2. Visual Studio 2022
3. Rider
4. C#

<br><br>

[☢️ 목차로 돌아가기](#-목차)

<br><br>

---

<br><br>

# 🧟: 기술적 고민과 트러블 슈팅
#### 클릭하면 자세한 내용을 확인하실 수 있습니다!🐰

### 기술적 고민
> 카메라에 플레이어가 가려졌을 때, 장애물 오브젝트를 반투명하게 만들어주고 싶었지만 Leagacy Shaders의 Transparent Diffuse 쉐이더로는 사용한 에셋의 깊이 정보가 반영되지 않아 부자연스럽게 보임
>
> 해결 : Shader의 영역은 알고 있는 팀원이 없었기에 튜터님에게 찾아가 원하는 상황을 말씀 드려 ChatGpt를 통해 Custom Shader 파일을 생성    
    
### 트러블 슈팅

##### Critical Issue!    

> 게임 플레이 중 랜덤하게 게임이 프리징 되는 현상 발생    
>    
> ![randomspawn](https://github.com/SnowScapes/ZCity_Public/assets/39547945/2afef978-e88a-41f0-82bc-542864090bfb)    
> 
> 원인 분석 : 랜덤한 자원을 선택하여 GameObject를 켜주는 반복문에 거치며 프리징이 걸리는 것으로 추정
> 
> 해결 방법 : 반복문 사용을 제거하고 Coroutine을 통해 랜덤한 자원이 선택되지 못했을 경우, 다음 프레임에 재시도 하는 방식으로 구조 변경
>
> Fixed!

<br><br>

[☢️ 목차로 돌아가기](#-목차)

<br><br>

---

<br><br>

## 🧟: 만든 사람들

> 팀장 : [김창민](https://github.com/KCngMn)    
> 팀원 : [이시원](https://github.com/SnowScapes)    
> 팀원 : [정해성](https://github.com/jelly1702)    
> 팀원 : [최재훈](https://github.com/chl1195)    

<br><br>

[☢️ 목차로 돌아가기](#-목차)

<br><br>
