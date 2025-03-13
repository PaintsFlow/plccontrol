# 자동차 도장 공정 MES Broker 중 PLC Control Program

WPF에서 PLC를 제어하기 위한 중계자 역할을 수행하는 C# Console Application입니다.

## 주요 기능

1. WPF 제어 정보 구독
2. PLC LED ON/OFF 제어

## 실행 방법

*Visual Studio 기반 프로젝트입니다.*
.NET 9.0 버전 필요.

1. <a href = "https://github.com/PaintsFlow/CarPaintingProcess">WPF</a> 프로젝트 실행
2. 본 레파지토리 Clone 후 실행
3. WPF 도장 공정 단계 제어 버튼 클릭

> 본 프로젝트는 실물 PLC 컨트롤을 위한 프로젝트입니다. Visual Studio 2022 기반


## 시연
![plccontrol](/img/5.PLC-ezgif.com-video-to-gif-converter.gif)

> 좌 WPF, 우 PLC(시뮬레이터)

## 사용 기술

1. dotnet 9.0
2. RabbitMQ
3. PLC modbus

---
기여 : **김혜영**
