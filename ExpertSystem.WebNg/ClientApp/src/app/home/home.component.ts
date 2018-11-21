import { Component } from '@angular/core';
import { InferenceState } from '../shared/inferecne-state';
import { EsKnowledge, EsParser } from '../shared/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private state: InferenceState;
  private knowledgeText = defautKnowledge;
  private knowledge: EsKnowledge;
  private target = 'семейство';
  private question: string;
  private questionVariants: string[];
  private oldTarget: string;
  private answer: string;

  public applyKnowledgeText() {
    this.knowledge = EsParser.ParseKnowledge(this.knowledgeText);
  }

  public analyze() {
    this.state = new InferenceState(this.knowledge, this.target);
    this.nextStep();
  }

  private nextStep() {
    this.state.Process();
    if (this.state.IsCompleted) {
      this.oldTarget = this.target;
      this.answer = this.state.Result;
      if (!this.answer) {
        this.answer = 'не найдено';
      }
      this.state = null;
    } else {
      this.question = this.state.UnknownFact;
      this.questionVariants = this.state.UnknownFactVariants;
    }
  }

  private nextAnswer(answer) {
    if (answer) {
      this.state.PushFactValue(answer);
    } else {
      this.state.PushFactValue(this.answer);
    }
    this.nextStep();
  }
}

const defautKnowledge = `
<если/>
класс = голосеменные
структура листа = чешуеобразная
<то/> семейство = кипарисовые


<если/>
класс = голосеменные
структура листа = иглоподобная
конфигурация = хаотическая
<то/> семейство = сосновые


<если/>
класс = голосеменные
структура листа = иглоподобная
конфигурация = 2 ровных ряда
серебристая полоска = да
<то/> семейство = еловые

<если/>
класс = голосеменные
структура листа = иглоподобная
конфигурация = 2 ровных ряда
серебристая полоска = нет
<то/> семейство = болотный кипарис

<если/>
тип = деревья
форма листа = широкая и плоская
<то/> класс = покрытосеменные

<если/>
тип = деревья
форма листа = не широкая и не плоская
<то/> класс = голосеменные

<если/> стебель = зеленый
<то/> тип = трявянистые

<если/> стебель = древесный ; положение = стелющееся
<то/> тип = лианы

<если/> стебель = древесный ; положение = прямостоящее ; один основной ствол = да
<то/> тип = деревья

<если/> стебель = древесный ; положение = прямостоящее ; один основной ствол = нет
<то/> тип = кустарниковые

<если/>
голова = болит
кости = ломит
глаза = слезяться
<то/> болезнь = дцп

<если/>
голова = в порядке
кости = в порядке
глаза = в порядке
<то/> болезнь = ты здоров`;
