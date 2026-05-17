using System.Text;
using UnityEngine;

public class JLPTPromptBuilder : MonoBehaviour
{
    private readonly string[] categories =
    {
        "kanji_reading",
        "notation",
        "word_formation",
        "context_usage",
        "synonym",
        "usage",
        "grammar",
        "sentence_assembly",
        "text_grammar"
    };

    public string BuildSystemPrompt()
    {
        return
            "You are a JLPT-inspired Japanese language knowledge quiz generator for study purposes. " +
            "You MUST output ONLY valid JSON. " +
            "Do NOT output markdown. " +
            "Do NOT output explanations. " +
            "Do NOT output code blocks. " +
            "Every quiz MUST have exactly 4 options and exactly 1 correct answer.";
    }

    public string BuildUserPrompt(string jlptLevel, int quizCount)
    {
        string categoryList = BuildRandomCategoryList(quizCount);
        string exampleBlock = GetExampleBlock(jlptLevel);

        return $@"
Generate {quizCount} Japanese language knowledge quizzes inspired by JLPT {jlptLevel} difficulty.

Each quiz MUST use the category assigned below.

Assigned categories in order:
{categoryList}

Category descriptions:
- kanji_reading: Ask for the correct reading of a kanji word.
- notation: Ask for the correct kanji or kana notation of an underlined word.
- word_formation: Ask which word best completes the sentence.
- context_usage: Ask which expression best fits the sentence context.
- synonym: Ask for an expression with a similar meaning.
- usage: Ask which sentence uses the given word correctly.
- grammar: Ask which grammar pattern correctly completes the sentence.
- sentence_assembly: Ask the learner to arrange sentence parts correctly.
- text_grammar: Ask a grammar question based on a very short passage.

Example quizzes:
The following are examples only. Do not copy them directly.
Use these examples only as a style, structure, and difficulty reference.

{exampleBlock}

Requirements:
- The quizzes should follow the general style and difficulty of JLPT-inspired language knowledge practice questions.
- Use Japanese only.
- Each quiz must have exactly 4 options.
- Each quiz must have exactly 1 correct answer.
- Incorrect choices must be believable and similar to the correct answer.
- Do not create duplicate options.
- Keep the difficulty appropriate for JLPT {jlptLevel}.
- Do not include explanations.
- Do not include romaji.
- Do not include English.
- Do not include markdown.
- The response must start with '{{' and end with '}}'.

Output ONLY this JSON format:

{{
  ""quizzes"": [
    {{
      ""category"": ""kanji_reading"",
      ""question"": ""..."",
      ""options"": [""..."", ""..."", ""..."", ""...""],
      ""answerIndex"": 0
    }}
  ]
}}
";
    }

    private string GetExampleBlock(string jlptLevel)
    {
        switch (jlptLevel)
        {
            case "N1":
                return GetN1Examples();
            case "N2":
                return GetN2Examples();
            case "N3":
                return GetN3Examples();
            case "N4":
                return GetN4Examples();
            case "N5":
            default:
                return GetN5Examples();
        }
    }

    private string GetN5Examples()
    {
        return @"
{
  ""category"": ""kanji_reading"",
  ""question"": ""「図書館」の読み方はどれですか。"",
  ""options"": [""としょかん"", ""としょうかん"", ""ずしょかん"", ""ずしょうかん""],
  ""answerIndex"": 0
}

{
  ""category"": ""notation"",
  ""question"": ""「がっこう」を漢字で書くとどれですか。"",
  ""options"": [""学校"", ""学生"", ""会社"", ""先生""],
  ""answerIndex"": 0
}

{
  ""category"": ""word_formation"",
  ""question"": ""毎日 日本語を ___。"",
  ""options"": [""勉強します"", ""病気です"", ""静かです"", ""便利です""],
  ""answerIndex"": 0
}

{
  ""category"": ""context_usage"",
  ""question"": ""あした テストが あります。きょうは ___ 勉強します。"",
  ""options"": [""たくさん"", ""あまり"", ""ぜんぜん"", ""すこしも""],
  ""answerIndex"": 0
}

{
  ""category"": ""synonym"",
  ""question"": ""「はやい」と 同じ 意味の ものは どれですか。"",
  ""options"": [""速い"", ""遅い"", ""高い"", ""近い""],
  ""answerIndex"": 0
}

{
  ""category"": ""usage"",
  ""question"": ""「借ります」の使い方として正しいものはどれですか。"",
  ""options"": [
    ""図書館で 本を 借ります。"",
    ""水を 借ります。"",
    ""ご飯を 借ります。"",
    ""学校を 借ります。""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""grammar"",
  ""question"": ""毎日 ７時 ___ 起きます。"",
  ""options"": [""に"", ""を"", ""で"", ""へ""],
  ""answerIndex"": 0
}

{
  ""category"": ""sentence_assembly"",
  ""question"": ""文を完成してください。わたしは [1.日本語を] [2.毎日] [3.勉強します]。"",
  ""options"": [
    ""2 → 1 → 3"",
    ""1 → 2 → 3"",
    ""3 → 1 → 2"",
    ""2 → 3 → 1""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""text_grammar"",
  ""question"": ""わたしは 毎朝 コーヒーを 飲みます。きょうも ___ 飲みました。"",
  ""options"": [""コーヒーを"", ""学校を"", ""電車を"", ""新聞を""],
  ""answerIndex"": 0
}
";
    }

    private string GetN4Examples()
    {
        return @"
{
  ""category"": ""kanji_reading"",
  ""question"": ""「急行」の読み方はどれですか。"",
  ""options"": [""きゅうこう"", ""きゅこう"", ""きょうこう"", ""きゅうぎょう""],
  ""answerIndex"": 0
}

{
  ""category"": ""notation"",
  ""question"": ""「しんせつ」を漢字で書くとどれですか。"",
  ""options"": [""親切"", ""新設"", ""先生"", ""新聞""],
  ""answerIndex"": 0
}

{
  ""category"": ""word_formation"",
  ""question"": ""この町は 静かで、とても ___ やすいです。"",
  ""options"": [""住み"", ""読み"", ""書き"", ""飲み""],
  ""answerIndex"": 0
}

{
  ""category"": ""context_usage"",
  ""question"": ""雨が 降りそうです。かさを ___ ほうがいいです。"",
  ""options"": [""持っていった"", ""持っていく"", ""持っている"", ""持っていた""],
  ""answerIndex"": 1
}

{
  ""category"": ""synonym"",
  ""question"": ""「だんだん」と 近い意味のものはどれですか。"",
  ""options"": [""少しずつ"", ""すぐに"", ""ぜんぜん"", ""いつも""],
  ""answerIndex"": 0
}

{
  ""category"": ""usage"",
  ""question"": ""「間に合います」の使い方として正しいものはどれですか。"",
  ""options"": [
    ""急げば 電車に 間に合います。"",
    ""毎日 朝ご飯に 間に合います。"",
    ""この本は 日本語に 間に合います。"",
    ""友だちを 間に合います。""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""grammar"",
  ""question"": ""この料理は からすぎて、わたしには ___。"",
  ""options"": [""食べられません"", ""食べませんでした"", ""食べてください"", ""食べたことです""],
  ""answerIndex"": 0
}

{
  ""category"": ""sentence_assembly"",
  ""question"": ""文を完成してください。駅へ [1.行く] [2.バスに] [3.つもりです] [4.乗って]。"",
  ""options"": [
    ""2 → 4 → 1 → 3"",
    ""1 → 2 → 4 → 3"",
    ""4 → 2 → 3 → 1"",
    ""2 → 1 → 4 → 3""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""text_grammar"",
  ""question"": ""昨日は 雨でした。だから、どこへも ___。"",
  ""options"": [""行きませんでした"", ""行きます"", ""行ってください"", ""行きたいです""],
  ""answerIndex"": 0
}
";
    }

    private string GetN3Examples()
    {
        return @"
{
  ""category"": ""kanji_reading"",
  ""question"": ""「確認」の読み方はどれですか。"",
  ""options"": [""かくにん"", ""かくねん"", ""かにん"", ""こうにん""],
  ""answerIndex"": 0
}

{
  ""category"": ""notation"",
  ""question"": ""「けいけん」を漢字で書くとどれですか。"",
  ""options"": [""経験"", ""計画"", ""経済"", ""見学""],
  ""answerIndex"": 0
}

{
  ""category"": ""word_formation"",
  ""question"": ""この機械の 使い方を ___ してください。"",
  ""options"": [""説明"", ""経験"", ""成功"", ""参加""],
  ""answerIndex"": 0
}

{
  ""category"": ""context_usage"",
  ""question"": ""電車が 遅れた ___、会議に 遅刻しました。"",
  ""options"": [""ため"", ""のに"", ""なら"", ""ほど""],
  ""answerIndex"": 0
}

{
  ""category"": ""synonym"",
  ""question"": ""「急に」と 近い意味のものはどれですか。"",
  ""options"": [""突然"", ""必ず"", ""十分"", ""最近""],
  ""answerIndex"": 0
}

{
  ""category"": ""usage"",
  ""question"": ""「延期する」の使い方として正しいものはどれですか。"",
  ""options"": [
    ""雨のため、試合を 延期することになった。"",
    ""ご飯を 延期して 食べた。"",
    ""駅まで 延期して 歩いた。"",
    ""友だちを 延期して 会った。""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""grammar"",
  ""question"": ""この仕事は 明日までに ___ なりません。"",
  ""options"": [""終わらせなければ"", ""終わらせても"", ""終わらせるなら"", ""終わらせたら""],
  ""answerIndex"": 0
}

{
  ""category"": ""sentence_assembly"",
  ""question"": ""文を完成してください。彼は [1.日本に] [2.来た] [3.ばかり] [4.です]。"",
  ""options"": [
    ""1 → 2 → 3 → 4"",
    ""2 → 1 → 3 → 4"",
    ""3 → 1 → 2 → 4"",
    ""1 → 3 → 2 → 4""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""text_grammar"",
  ""question"": ""田中さんは 毎日 運動している。そのため、とても ___。"",
  ""options"": [""健康そうだ"", ""健康だったら"", ""健康なのに"", ""健康ではない""],
  ""answerIndex"": 0
}
";
    }

    private string GetN2Examples()
    {
        return @"
{
  ""category"": ""kanji_reading"",
  ""question"": ""「詳細」の読み方はどれですか。"",
  ""options"": [""しょうさい"", ""しょうざい"", ""しょさい"", ""じょうさい""],
  ""answerIndex"": 0
}

{
  ""category"": ""notation"",
  ""question"": ""「えんき」を漢字で書くとどれですか。"",
  ""options"": [""延期"", ""演技"", ""縁起"", ""遠慮""],
  ""answerIndex"": 0
}

{
  ""category"": ""word_formation"",
  ""question"": ""新しい制度を ___ するには、十分な準備が必要だ。"",
  ""options"": [""導入"", ""納得"", ""比較"", ""集中""],
  ""answerIndex"": 0
}

{
  ""category"": ""context_usage"",
  ""question"": ""会議の内容を忘れないように、重要な点を ___ おいた。"",
  ""options"": [""メモして"", ""メモすると"", ""メモしたら"", ""メモすれば""],
  ""answerIndex"": 0
}

{
  ""category"": ""synonym"",
  ""question"": ""「わずか」と 近い意味のものはどれですか。"",
  ""options"": [""ほんの少し"", ""かなり多く"", ""ちょうどよく"", ""ほとんど全部""],
  ""answerIndex"": 0
}

{
  ""category"": ""usage"",
  ""question"": ""「踏まえる」の使い方として正しいものはどれですか。"",
  ""options"": [
    ""調査結果を踏まえて、計画を見直す。"",
    ""靴を踏まえて、外へ出る。"",
    ""昼食を踏まえて、会議を食べる。"",
    ""駅を踏まえて、電車に乗る。""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""grammar"",
  ""question"": ""彼は 忙しい ___、毎日 日本語の勉強を続けている。"",
  ""options"": [""にもかかわらず"", ""にしたがって"", ""に対して"", ""について""],
  ""answerIndex"": 0
}

{
  ""category"": ""sentence_assembly"",
  ""question"": ""文を完成してください。成功するかどうかは [1.努力] [2.次第] [3.本人の] [4.だ]。"",
  ""options"": [
    ""3 → 1 → 2 → 4"",
    ""1 → 3 → 2 → 4"",
    ""2 → 3 → 1 → 4"",
    ""3 → 2 → 1 → 4""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""text_grammar"",
  ""question"": ""この商品は 高い。しかし、品質を考えれば ___。"",
  ""options"": [""買う価値がある"", ""買うしかないとは限らない"", ""買ったところだ"", ""買わずにはいられない""],
  ""answerIndex"": 0
}
";
    }

    private string GetN1Examples()
    {
        return @"
{
  ""category"": ""kanji_reading"",
  ""question"": ""「緻密」の読み方はどれですか。"",
  ""options"": [""ちみつ"", ""せいみつ"", ""ちひつ"", ""しみつ""],
  ""answerIndex"": 0
}

{
  ""category"": ""notation"",
  ""question"": ""「けねん」を漢字で書くとどれですか。"",
  ""options"": [""懸念"", ""記念"", ""概念"", ""兼任""],
  ""answerIndex"": 0
}

{
  ""category"": ""word_formation"",
  ""question"": ""彼の説明には ___ があり、すぐには納得できなかった。"",
  ""options"": [""矛盾"", ""均衡"", ""妥協"", ""余地""],
  ""answerIndex"": 0
}

{
  ""category"": ""context_usage"",
  ""question"": ""その政策は、国民の生活に大きな影響を ___。"",
  ""options"": [""及ぼしかねない"", ""及ぼしようがない"", ""及ぼすに限る"", ""及ぼすまでもない""],
  ""answerIndex"": 0
}

{
  ""category"": ""synonym"",
  ""question"": ""「おおむね」と 近い意味のものはどれですか。"",
  ""options"": [""だいたい"", ""まれに"", ""むやみに"", ""ただちに""],
  ""answerIndex"": 0
}

{
  ""category"": ""usage"",
  ""question"": ""「あながち」の使い方として正しいものはどれですか。"",
  ""options"": [
    ""彼の意見もあながち間違いとは言えない。"",
    ""私はあながち駅へ行った。"",
    ""この本はあながち机の上にある。"",
    ""彼はあながち昼ご飯を食べた。""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""grammar"",
  ""question"": ""彼の発言は、誤解を招き ___ 表現だった。"",
  ""options"": [""かねない"", ""ようがない"", ""きれない"", ""ざるをえない""],
  ""answerIndex"": 0
}

{
  ""category"": ""sentence_assembly"",
  ""question"": ""文を完成してください。この問題は [1.専門家で] [2.さえ] [3.判断が] [4.難しい]。"",
  ""options"": [
    ""1 → 2 → 3 → 4"",
    ""2 → 1 → 3 → 4"",
    ""3 → 1 → 2 → 4"",
    ""1 → 3 → 2 → 4""
  ],
  ""answerIndex"": 0
}

{
  ""category"": ""text_grammar"",
  ""question"": ""計画は十分に検討された。しかし、実行段階で問題が生じないとは ___。"",
  ""options"": [""限らない"", ""限る"", ""限っている"", ""限りだ""],
  ""answerIndex"": 0
}
";
    }

    private string BuildRandomCategoryList(int quizCount)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < quizCount; i++)
        {
            string category = categories[Random.Range(0, categories.Length)];
            sb.AppendLine($"{i + 1}. {category}");
        }

        return sb.ToString();
    }
}