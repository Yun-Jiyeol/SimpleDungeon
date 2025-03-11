# SIMPLE DUNGEON

1. 플레이어 컨트롤
   
![image](https://github.com/user-attachments/assets/54b8e8cd-7b8e-46c3-b7f5-4fa04629c57e)
![image](https://github.com/user-attachments/assets/92888478-82b8-4ad3-9000-ad626b5025af)

Input System을 통한 제작

2. 포션

![스크린샷 2025-03-11 092813](https://github.com/user-attachments/assets/734d750b-b32e-465f-8d74-04bb1560dd10)

총 3종류의 포션이 존재. 회복, 1회용 추가 점프, 순간 이속증가

3. 장비

   ![스크린샷 2025-03-11 092848](https://github.com/user-attachments/assets/f51995a6-eca3-43c3-bcdb-19fd329ebd18)
![스크린샷 2025-03-11 092906](https://github.com/user-attachments/assets/5841e5b6-c3ef-4066-812c-25a54bd4977d)

2종류의 장비 존재. 획득 시 일정 시간동안 사용가능. 장착동안 설정된 자원을 획득할 수 있도록 도움을 준다.

4. Carryable 물체

![image](https://github.com/user-attachments/assets/9f1330f5-6349-4f0b-a9d2-58129320c0d9)
  ![image](https://github.com/user-attachments/assets/52f34c18-cd13-4ae8-8fee-1a20fe1ee52a)

클릭 시 들 수 있으며 들고 있는 상태에서 1번 더 클릭 시 이를 멀리 던진다

5. Holdable 물체

![image](https://github.com/user-attachments/assets/1a622c20-e328-4751-b2aa-e5db1632cdc6)

클릭 시 그 지점 가까이까지 움직이며 추가적인 높은 점프를 1회 더 할 수 있다.

6. 점프대

![image](https://github.com/user-attachments/assets/c7d909ca-cdec-41d5-a9d3-a0335c7f7f4c)

올라가 있으면 N초 후 자동으로 높이 점프를 한다

7. 함정

![image](https://github.com/user-attachments/assets/2701709c-b226-44ae-bcf6-36171fc7d552)

Ray를 통한 확인으로 아래에 깔릴 시 뒤로 밀리며 데미지를 받고 위에서는 밟고 올라갈 수 있다.

8. 레버

![image](https://github.com/user-attachments/assets/e1ce73f1-09b4-4999-9743-b9e2974eb8b2)
![image](https://github.com/user-attachments/assets/ca44e13b-16f8-4c5c-a3af-214e7d680e5e)


누르고 있으면 움직일 수 없으며 일정 시간동안 꾹 누르면 당겨지며 설정된 양만큼의 전기를 흐르게 합니다.

9. 적

![스크린샷 2025-03-10 203114](https://github.com/user-attachments/assets/2a28b915-201c-40cf-b273-74ac0a88d97a)

Find - 플레이어를 찾을 때까지 현제 좌표에서 가까운 임의의 좌표로 계속 이동합니다.
Question - Ray를 통해 멀리 플레이어를 인식하면 의문을 가지며(순간 정지) 처음 인식한 좌표를 확인하러 움직입니다. 만약 찾지 못하면 다시 Find로 돌아갑니다.
Follow - 일정 범위에서 플레이어를 인식하면 빠른 속도로 플레이어를 계속 따라옵니다.
Attack - 공격 사거리에 도달하면 정지하여 플레이어를 바라보며 공격합니다.

완벽하게 제작되지는 않았지만 이 4개의 상태를 상황에 따라 자동으로 번갈아가며 동작합니다.
