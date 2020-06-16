using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Actions
{
    public class ActionFactory
    {
        protected ActionFactory()
        {
            _AddFactoryMethod<ChanceAction>(() => { return new ChanceAction(); });
            _AddFactoryMethod<CheckDayAction>(() => { return new CheckDayAction(); });
            _AddFactoryMethod<FateAction>(() => { return new FateAction(); });
            _AddFactoryMethod<GiveChildAction>(() => { return new GiveChildAction(); });
            _AddFactoryMethod<HealthAction>(() => { return new HealthAction(); });
            _AddFactoryMethod<RiskAction>(() => { return new RiskAction(); });
            _AddFactoryMethod<CharityAction>(() => { return new CharityAction(); });
            _AddFactoryMethod<StudyAction>(() => { return new StudyAction(); });

            _AddFactoryMethod<FreeChoiceAction>(() => { return new FreeChoiceAction(); });
            _AddFactoryMethod<InnerCheckDayAction>(() => { return new InnerCheckDayAction(); });
            _AddFactoryMethod<InnerFateAction>(() => { return new InnerFateAction(); });
            _AddFactoryMethod<InnerHealthAction>(() => { return new InnerHealthAction(); });
            _AddFactoryMethod<InvestAction>(() => { return new InvestAction(); });
            _AddFactoryMethod<QualityAction>(() => { return new QualityAction(); });
            _AddFactoryMethod<RelaxAction>(() => { return new RelaxAction(); });
            _AddFactoryMethod<InnerStudyAction>(() => { return new InnerStudyAction(); });
        }

        public T Create<T>(Player owner) where T : ActionBase
        {
            Type p = typeof(T);
            FactoryMethodDelegate func;
            if (!_factories.TryGetValue(p.Name, out func))
            {
                Console.Error.WriteLine("[ActionFactory.Create] error: this is no {0}", p);
            }

            var action = func();
            action.owner = owner;
            return action as T;
        }

        public ActionBase Create(string type, Player owner)
        {
            FactoryMethodDelegate func;
            if (!_factories.TryGetValue(type, out func))
            {
                Console.Error.WriteLine("[ActionFactory.Create] error: this is no {0}", type);
            }

            var action = func();
            action.owner = owner;
            return action;
        }

        private void _AddFactoryMethod<T>(FactoryMethodDelegate method) where T : ActionBase
        {
            Type p = typeof(T);
            _factories.Add(p.Name, method);
        }
        
        public delegate ActionBase FactoryMethodDelegate();
        public static ActionFactory Instance = new ActionFactory();
        protected readonly Dictionary<string, FactoryMethodDelegate> _factories = new Dictionary<string, FactoryMethodDelegate>();
    }
}
