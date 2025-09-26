# Crockhead.Core

## 개요
경량의 C# 기반 기본 라이브러리로, 구조화되고 재사용 가능한 핵심 구성 요소들을 제공합니다.   
.NET Standard 2.1을 대상으로 하며 외부 종속성이 없습니다.   

## 배포
[![NuGet version](https://img.shields.io/nuget/v/Crockhead.Core.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Crockhead.Core/)
[![NuGet downloads](https://img.shields.io/nuget/dt/Crockhead.Core.svg?style=flat)](https://www.nuget.org/packages/Crockhead.Core/)

Lightweight C# base library providing structured and reusable core components.   
Targets .NET Standard 2.1 with no external dependencies.   

## 설치
~~~bash
dotnet add package Crockhead.Core
~~~
~~~powershell
Install-Package Crockhead.Core
~~~

## 라이브러리 기반
- netstandard2.1   

## 네임스페이스
~~~cs
using Crockhead.Core;
using Crockhead.Logging;
~~~

## 디렉토리 별 기능 목록 (최신 버전 기준)
|항목|설명|
|---|---|
|Base|명시적으로 해제 가능한 일회용 객체 및 리플랙션 기능 등.|
|Collection|쓰레드 안전성이 보장된 락 기반의 동적배열 자료구조.|
|Event|필수적인 메서드 인보케이션 기능만 존재하는 경량 이벤트 통보.|
|Identification|다양한 종류의 고유식별자 생성 및 관리.|
|Journal|다양한 기능의 로깅.|
|Logging|필수적인 기능만 존재하는 경량 로깅.|   
|Notification|다양한 기능의 이벤트 통보.|
|Operation|다양한 처리 명령. (비동기 명령 처리, 순차적인 명령 보류 처리 등)|
|Registration|타입 별 고유 인스턴스 등록 및 보관 기능.| 
|Shared|인스턴스 캐시 및 싱글톤 기능 등.|
