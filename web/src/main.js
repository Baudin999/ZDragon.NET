import App from "./App.svelte";

const app = new App({
  target: document.body,
  props: {
    name: "world"
  }
});

export default app;

if (!Prism || !Prism.languages) {
  debugger;
}
Prism.languages.carlang = {
  prolog: /<\?[\s\S]+?\?>/,
  keyword: /\b(?:type|alias|choice|data|flow|view|compose|loop)\b/,
  number: /\b0x[\da-f]+\b|(?:\b\d+\.?\d*|\B\.\d+)(?:e[+-]?\d+)?/i,
  operator: /[<>]=?|[!=]=?=?|--?|\+\+?|&&?|\|\|?|[?*/~^%]/,
  chapter: /#.*\n/
};
