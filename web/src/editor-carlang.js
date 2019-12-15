export const tokenizer = {
  brackets: [{ open: "{*", close: "*}", token: "delimiter.bracket" }],
  keywords: ["type", "alias", "choice", "view", "flow", "data"],
  autoClosingPairs: [{ open: "{*", close: "*}" }],
  digits: /\d+(_+\d+)*/,
  tokenizer: {
    root: [{ include: "@chapter" }, { include: "@annotation" }, { include: "common" }],
    chapter: [[/#.*/, "chapter"]],
    annotation: [[/@.*/, "annotation"]],
    common: [
      [
        /^.[a-z$][\w$]*/,
        {
          cases: {
            "@keywords": "keyword",
            "@default": "identifier"
          }
        }
      ],
      [/(@digits)n?/, "number"]
    ]
  }
};

export const theme = {
  base: "vs",
  inherit: true,
  rules: [
    { token: "chapter", foreground: "#ea5dd5" },
    { token: "annotation", foreground: "#800000" }
  ]
};

export const createCommands = (monaco, editor) => {
  editor.addCommand(monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_P, (e, a, b) => {
    var model = editor.getModel();
    var position = editor.getPosition();
    var { word } = model.getWordAtPosition(position);
    alert(word);
  });
};

export const createActions = (monaco, editor) => {
  editor.addAction({
    // An unique identifier of the contributed action.
    id: "zdragon_create_type",

    // A label of the action that will be presented to the user.
    label: "Create Type",

    // An optional array of keybindings for the action.
    keybindings: [monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_E],

    // A precondition for this action.
    precondition: null,

    // A rule to evaluate on top of the precondition in order to dispatch the keybindings.
    keybindingContext: null,

    contextMenuGroupId: "navigation",

    contextMenuOrder: 1.5,

    // Method that will be executed when the action is triggered.
    // @param editor The editor instance is passed in as a convinience
    run: function(ed) {
      var baseTypes = ["String", "Number", "Date", "DateTime", "Time", "Decimal", "List", "Maybe"];
      var position = editor.getPosition();
      var text = editor.getValue(position);
      var model = editor.getModel();
      var line = model.getLineContent(position.lineNumber);
      var { word } = model.getWordAtPosition(position);

      var newtext;
      if (baseTypes.indexOf(word) == -1) {
        newtext =
          text +
          `
type ${word}
`;
      } else {
        let type = line.split(":").map(s => s.trim())[0];
        newtext =
          text +
          `
alias ${type} = ${word};
`;
      }
      editor.setValue(newtext);
      editor.setPosition(position);
      return null;
    }
  });
};

// export const completionProvider = (monaco, editor) => {
//     //
// }
