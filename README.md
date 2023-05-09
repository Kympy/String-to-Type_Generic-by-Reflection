# String to Type_Generic by Reflection
 Convert stringTypeName to Generic Type by C# Reflection
 
 스트링형태의 타입이름을 실제 타입으로 변경 후
 런타임 중에 동적으로 Generic Method 를 생성하고 사용한다.
 
 N^3 의 복잡도를 가지며,
 병렬 처리 시 더 빠른 속도를 얻을 수 있지만,
 솔루션 내에 존재하는 Assembly의 숫자가 많을 수록 기하급수적으로 느려짐.
 
 R&D 필요
