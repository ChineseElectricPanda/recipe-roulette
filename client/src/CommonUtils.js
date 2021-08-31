const domParser = new DOMParser();

export function decodeHtml(input)
{
  return domParser.parseFromString(input, 'text/html').documentElement.textContent;
}