# TitleSystem

이 프로젝트는 .NET과 SignalR을 기반으로 한 실시간 클라이언트-서버 애플리케이션입니다. 서버(`TitleServer`)는 클라이언트(`TitleConsole`)의 연결을 수신하고 실시간으로 통신합니다.

## 프로젝트 구조

이 솔루션은 다음과 같은 네 개의 주요 프로젝트로 구성됩니다:

- **TitleServer**: ASP.NET Core SignalR 허브를 호스팅하는 메인 서버 애플리케이션입니다. `ServerCore` 프로젝트의 로직을 사용합니다.
- **ServerCore**: 서버 측의 핵심 비즈니스 로직과 데이터 처리를 담당하는 클래스 라이브러리입니다.
- **TitleConsole**: 서버와 통신하는 .NET 콘솔 클라이언트 애플리케이션입니다. `ClientCore` 프로젝트를 활용하여 서버에 연결하고 메시지를 주고받습니다.
- **ClientCore**: 클라이언트 측의 서버 연결 및 통신 로직을 캡슐화한 클래스 라이브러리입니다.

## 실행 방법

### 사전 요구사항

- .NET 10.0 SDK (또는 프로젝트에 명시된 버전)

### 1. 서버 실행

터미널을 열고 `TitleServer` 디렉토리로 이동한 후 다음 명령어를 실행합니다.

```bash
cd TitleServer
dotnet run
```

### 2. 클라이언트 실행

**새로운** 터미널을 열고 `TitleConsole` 디렉토리로 이동한 후 다음 명령어를 실행합니다.

```bash
cd TitleConsole
dotnet run
```

## 주요 기술

- C# & .NET 10.0
- ASP.NET Core
- ASP.NET Core SignalR
