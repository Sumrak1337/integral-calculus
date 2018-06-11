using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace SumrakBot
{ 
    class Program
    {
        static VkApi vkapi = new VkApi();
        static long userID = 0;
        static ulong? Ts;
        static ulong? Pts;
        static bool IsActive;
        static Timer WatchTimer = null;
        static byte MaxSleepSteps = 3;
        static int StepSleepTime = 333;
        static byte CurrentSleepSteps = 1;
        delegate void MessagesRecievedDelegate(VkApi owner, ReadOnlyCollection<Message> messages);
        static event MessagesRecievedDelegate NewMessages;

        static Func<string> DoubleCode = () =>
        {
            Console.Write("Введите код авторизации: ");
            return Console.ReadLine();
        };

        static void Main(string[] args)
        {
            string Login = File.ReadAllText(@"C:\Users\пк\Desktop\vk\VkBot\login.txt");
            string Password = File.ReadAllText(@"C:\Users\пк\Desktop\vk\VkBot\password.txt");
            ulong ID = ulong.Parse(File.ReadAllText(@"C:\Users\пк\Desktop\vk\VkBot\ID.txt"));
            bool DoubleAuth = false;
            ConsoleStyle();

            Console.WriteLine("Выполняется вход...");

            if (Auth(Login, Password, ID, DoubleAuth))
            {
                ColorMessage("Выполнен вход.", ConsoleColor.Green);
                User I = vkapi.Users.Get(vkapi.UserId.Value);
                Console.WriteLine("Вы: " + I.FirstName + " " + I.LastName);
                Eye();
                ColorMessage("Слежение за сообщениями включено.", ConsoleColor.Yellow);
            }
            else
            {
                ColorMessage("Неверный логин или пароль!", ConsoleColor.Red);
            }

            Console.WriteLine("Нажмите ENTER чтобы выйти.");
            Console.ReadLine();
        }

        static void ConsoleStyle()
        {
            Console.Title = "SumrakBot";
            ColorMessage("SumrakBot", ConsoleColor.Blue);
        }

        static void ColorMessage(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static bool Auth(string Login, string Password, ulong ID, bool HasCode)
        {
            try
            {
                if (HasCode == true)
                {
                    vkapi.Authorize(new ApiAuthParams
                    {
                        Login = Login,
                        Password = Password,
                        Settings = Settings.All,
                        ApplicationId = ID,
                        TwoFactorAuthorization = DoubleCode
                    });
                    return true;
                }
                else
                {
                    vkapi.Authorize(new ApiAuthParams
                    {
                        Login = Login,
                        Password = Password,
                        Settings = Settings.All,
                        ApplicationId = ID
                    });
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        static string[] Commands = { "помощь", "раздел1", "раздел2", "раздел3", "раздел4", "раздел5", "раздел6", "раздел7", "раздел8", "раздел9", "раздел10"};
        static void Command(string Message)
        {
            Message = Message.ToLower();
            if (Message == Commands[0])
            {
                SendMessage("Основы интегрального исчисления. Неопредленный интеграл. \n Введите номер интересующего Вас раздела в формате 'Раздел*': \n \n Раздел1: Первообразная, неопределенный интеграл и их свойства. \n Раздел2: Замена переменной, линейная замена и подстановка. \n Раздел3: Интегрирование по частям. \n Раздел4: Интегрирование простейших рациональных дробей; реккурентная формула. \n Раздел5: Основные сведения из алгебры многочленов; разложение на множители (C и R коэффициенты) \n Раздел6: Разложение правильных рациональных дробей на простейшие (случай R корней); аналитический метод отыскания коэффициентов разложения. \n Раздел7: Разложение правильных рациональных дробей на простейшие (случай С корней) \n Раздел8: Интегрирование рациональных дробей – метод Остроградского. \n Раздел9: Интегрирование рациональных тригонометрических выражений. \n Раздел10: Интегрирование иррациональных выражений. \n");
            }
            else if (Message == Commands[1])
            {
                SendMessage("Первообразная, неопределенный интеграл и их свойства. \n \n Функция F(x) называется первообразной для f(x), если F'(x) = f(x) \n Свойства: \n 1) если F(x) - первообразная для f(x), то F(x) + C - тоже. \n 2) если F(x) и G(x) - две различные первообразные для одной и той же функции f(x), то F(x) = G(x) + C \n \n Совокупность всех первообразных функций - неопределенный интеграл. \n ∫f(x)dx = F(x) + C, где \n F(x) - первообразная f(x), \n C - произвольная постоянная. \n Свойства: \n 1) ∫(f±g)dx = ∫fdx ± ∫gdx \n 2) ∫kf(x)dx = k∫f(x)dx \n \n");
            }
            else if (Message == Commands[2])
            {
                SendMessage("Замена переменной, линейная замена и подстановка. \n \n Теорема о замене переменной: \n Пусть f(x) непр., x=φ(t) - непр.-диф.-ема, => \n ∫f(x)dx = ∫f(φ(t))*φ'(t)dt \n \n Случай линейной подстановки: \n ∫f(kx+c)dx = 1/k∫f(kx+c)d(kx+c) \n \n");
            }
            else if (Message == Commands[3])
            {
                SendMessage("Интегрирование по частям. \n \n Теорема: \n Пусть u,v - непр.-диф-емы, => ∫uv'dx = uv - ∫u'vdx");
            }
            else if (Message == Commands[4])
            {
                SendMessage("Интегрирование простейших рациональных дробей; реккурентная формула. \n \n 1) ∫dx/(x-x0) = ln|x-x0| + C \n 2) ∫dx/(x-x0)^k = 1/(1-k)*1/(x-x0)^(k-1) + C \n 3) ∫(Mx+N)/(x^2+px+q) = M/2*ln|x^2+px+q| + (N-Mp/2)/sqrt(q-p^2/4)*atan((x+p/2)/sqrt(q-p^2/4)) + C \n 4) ∫(Mx+N)/(x^2+px+q)^k = M/2*(x^2+px+q)^(1-k)/(1-k) + (N-Mp/2)*∫dx/(x^2+px+q)^(k-1) + С");
            }
            else if (Message == Commands[5])
            {
                SendMessage("Основные сведения из алгебры многочленов; разложение на множители (C и R коэффициенты) \n \n 1) Основная Теорема Алгебры. \n 2) Теорема Безу \n 3) Следствие из Теоремы Безу (разложение по корням) \n \n Случай вещественных многочленов: \n Корни, в общем случае, могут быть комплексными. \n 1) Следствие из Основной Теоремы Алгебры: (C и С' корни) \n 2) Следствие из Теоремы Безу (разложение по корням) \n 3) Разложение вещественного многочлена на множители. \n x1...xk - различные R корни кратности l1...lk; \n z1...zm - различные С корни кратности s1...sm; \n => \n P(x) = An(x-x1)^l1...(x-xk)^lk(x^2+b1x+c1)^s1...(x^2+bmx+cm)^sm");
            }
            else if (Message == Commands[6])
            {
                SendMessage("Разложение правильных рациональных дробей на простейшие (случай R корней); аналитический метод отыскания коэффициентов разложения. \n \n Приведенное рациональное выражение Qm(x)/Pn(x) – правильное, если m < n, неправильное, если m >= n \n Если Qm(x)/Pn(x)- неправильное, то его всегда можно привести  к сумме \n Qm(x)/Pn(x) = Rm-n + Q’n-1(x)/Pn(x) (n-1 или меньше) \n \n Лемма: \n Пусть Qm(x)/Pn(x) – несократима \n x0 – корень Pn(x) кратности и Qm(x0) != 0,  тогда \n Qm(x)/Pn(x) = A/(x-x0)^k + Rl(x)/((x-x0)^(k-1)*Pn-k(x)), при этом l < n – 1 \n Следствие: k раз применить Лемму. \n Отыскание коэффициентов. \n Первый способ: привести все к общему знаменателю \n Второй способ: дифференцировать.");
            }
            else if (Message == Commands[7])
            {
                SendMessage("Разложение правильных рациональных дробей на простейшие (случай С корней) \n \n Лемма: Qk(x)/Pn(x) = (Ax+B)/(x^2+bx+c)^l + Rk-2(x)/((x^2+bx+c)^(l-1)*P’n-2l(x)) \n Следствие: L раз применить Лемму. \n Вычисление коэффициентов – только приводить к общему знаменателю.");
            }
            else if (Message == Commands[8])
            {
                SendMessage("Интегрирование рациональных дробей – метод Остроградского. \n \n ∫Qk(x)/Pn(x) = ∫Q’k’(x)/P’n’(x) + A1ln|x-x1|+…+Akln|x-xk|+ \n + B1ln(x^2+b1x+c1)+…+Bmln(x^2+bmx+cm)+ \n +C1atan((2x+b)/2d)+…+Cmatan((2x+b)/2d) + C, \n где d = sqrt(c – b^2/4)");
            }
            else if (Message == Commands[9])
            {
                SendMessage("Интегрирование рациональных тригонометрических выражений. \n \n 1)Целые выражения \n ∫sin(x)^n*cos(x)^mdx  \n a) n = 2k+1 => cos(x) = t \n б) m = 2k+1 => sin(x) = t \n в) n = 2k, m = 2l => свести к формуле понижения степени \n \n 2) Рациональные выражения \n R(sin(x), cos(x)) = Qk(sin(x),cos(x))/Pm(sin(x),cos(x)) \n a) R(-sin(x), cos(x)) = - R(sin(x),cos(x)) => cos(x) = t \n б) R(sin(x), -cos(x)) = - R(sin(x),cos(x)) => sin(x) = t \n \n 3) R(-sin(x),-cos(x)) = R(sin(x), cos(x)) => t = tan(x) \n \n 4) Универсальная замена через tan(x/2)");
            }
            else if (Message == Commands[10])
            {
                SendMessage("Интегрирование иррациональных выражений. \n Квадратичные: \n ∫R(x, sqrt(ax^2+bx+c))dx \n ax^2+bx+c = ±a’^2((px+q)±1) \n a) a < 0, D > 0 => px + q = sin(t) \n б) a > 0, D < 0 => px + q = sh(t) \n в) a > 0, D > 0 => px + q = ch(t) \n \n Алгебраические: ∫R(x, ((ax+b)/(cx+d))^(p1/q1),…,((ax+b)/(cx+d))^(pk/qk))dx => \n t^r = (ax+b)/(cx+d), где \n r =НОК(q1,…,qk) \n \n Биномиальные: \n∫x^p(ax^q+b)^rdx = ∫y^s(y+b)^rdy \n 1) s, r ∈ Q, s ∈ Z => t^q’ = y+b \n 2) s,r ∈ Q, r ∈ Z => t^q’ = y \n 3) s,r ∈ Q ; если (s+r) ∈ Z => R(y, ((y+b)/y)^r) \n \n Теорема Чебышева: \n Ни при каких других рациональных степенях s и r биномиальный дифференциал не интегрируется.");
            }
            else
            {
                SendMessage("Неизвестная команда. Введите 'Помощь' для вызова списка команд.");
            }
        }

        static void SendMessage(string Body)
        {
            try
            {
                vkapi.Messages.Send(new MessagesSendParams
                {
                    UserId = userID,
                    Message = Body
                });
            }
            catch(Exception e)
            {
                ColorMessage("Ошибка! " + e.Message, ConsoleColor.Red);
            }
            
        }

        static void Eye()
        {
            LongPollServerResponse Pool = vkapi.Messages.GetLongPollServer(true);
            StartAsync(Pool.Ts, Pool.Pts);
            NewMessages += Watcher_NewMessages;
        }

        static void Watcher_NewMessages(VkApi owner, ReadOnlyCollection<Message> messages)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i].Type != MessageType.Sended)
                {
                    User Sender = vkapi.Users.Get(messages[i].UserId.Value);
                    //Console.WriteLine("Новое сообщение: {0} {1}: {2}", Sender.FirstName, Sender.LastName, messages[i].Body);
                    userID = messages[i].UserId.Value;
                    //Console.Beep();
                    Command(messages[i].Body);
                }
            }
        }

        static LongPollServerResponse GetLongPoolServer(ulong? lastPts = null)
        {
            LongPollServerResponse response = vkapi.Messages.GetLongPollServer(false, lastPts == null);
            Ts = response.Ts;
            Pts = Pts == null ? response.Pts : lastPts;
            return response;
        }

        static Task<LongPollServerResponse> GetLongPoolServerAsync(ulong? lastPts = null)
        {
            return Task.Run(() => 
            {
                return GetLongPoolServer(lastPts);
            });
        }

        static LongPollHistoryResponse GetLongPoolHistory()
        {
            if (!Ts.HasValue) GetLongPoolServer(null);
            MessagesGetLongPollHistoryParams rp = new MessagesGetLongPollHistoryParams();
            rp.Ts = Ts.Value;
            rp.Pts = Pts;
            int i = 0;
            LongPollHistoryResponse history = null;
            string errorLog = "";

            while (i < 5 && history == null)
            {
                i++;
                try
                {
                    history = vkapi.Messages.GetLongPollHistory(rp);
                }
                catch (TooManyRequestsException)
                {
                    Thread.Sleep(150);
                    i--;
                }
                catch (Exception ex)
                {                    
                    errorLog += string.Format("{0} - {1}{2}", i, ex.Message, Environment.NewLine);
                }
            }

            if (history != null)
            {
                Pts = history.NewPts;
                foreach (var m in history.Messages)
                {
                    m.FromId = m.Type == MessageType.Sended ? vkapi.UserId : m.UserId;
                }                    
            }
            else ColorMessage(errorLog, ConsoleColor.Red);
            return history;
        }

        static Task<LongPollHistoryResponse> GetLongPoolHistoryAsync()
        {
            return Task.Run(() => { return GetLongPoolHistory(); });
        }

        static async void WatchAsync(object state)
        {
            LongPollHistoryResponse history = await GetLongPoolHistoryAsync();
            if (history.Messages.Count > 0)
            {
                CurrentSleepSteps = 1;
                NewMessages?.Invoke(vkapi, history.Messages);
            }
            else if (CurrentSleepSteps < MaxSleepSteps) CurrentSleepSteps++;
            WatchTimer.Change(CurrentSleepSteps * StepSleepTime, Timeout.Infinite);
        }

        static async void StartAsync(ulong? lastTs = null, ulong? lastPts = null)
        {
            if (IsActive) ColorMessage("Messages for {0} already watching", ConsoleColor.Red);
            IsActive = true;
            await GetLongPoolServerAsync(lastPts);
            WatchTimer = new Timer(new TimerCallback(WatchAsync), null, 0, Timeout.Infinite);
        }

        static void Stop()
        {
            if (WatchTimer != null) WatchTimer.Dispose();
            IsActive = false;
            WatchTimer = null;
        }
    }
}