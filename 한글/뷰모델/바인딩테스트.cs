using System;
using 한글.공통기능;

namespace 한글.뷰모델
{
    public class 바인딩테스트 : 바인딩베이스
    {
        public 바인딩테스트()
        {
            이름 = "김심쿵";
            이름바꾸기 = new Command(() => 이름 = "김영환");
        }

        private string _이름;

        public string 이름
        {
            get { return _이름; }
            set { SetProperty(ref _이름, value); }
        }

        public Command 이름바꾸기 { get; set; }
    }
}
