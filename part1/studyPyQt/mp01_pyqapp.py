# PyQt 복습 - 직접 디자인 코딩
import sys
from PyQt5.QtWidgets import *


class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        self.initUI()

    def initUI(self):
        self.lblMessage = QLabel('메시지: ', self)
        self.lblMessage.setGeometry(10, 10, 350, 50)

        btnOk = QPushButton('Ok', self)
        btnOk.setGeometry(280, 250, 100, 40)
        # PyQt 이벤트(시그널): 버튼 누르는 것 -> 이벤트 핸들러(슬롯): 버튼 눌렀을 때 처리하는 것
        btnOk.clicked.connect(self.btnOk_clicked)

        self.setGeometry(300, 200, 400, 300)
        self.setWindowTitle('복습PyQt')
        self.show()
    
    def btnOk_clicked(self):
        self.lblMessage.clear()
        self.lblMessage.setText('메시지: OK!!!')


if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp() # MyApp()
    sys.exit(app.exec_())



