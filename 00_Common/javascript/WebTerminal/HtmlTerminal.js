/**
 * @class HtmlTerminal
 * 
 * This class is a very basic implementation of a "terminal" in the browser.
 * It provides simple functions like "write" and an "input" Callback.
 * 
 * @license AGPL-2.0
 * @author Alexaner Wunschik <https://github.com/mojoaxel>
 */
class HtmlTerminal {

  /**
   * Input callback.
   * If the prompt is activated by calling the input function
   * a callback is defined. If this member is not set this means
   * the prompt is not active.
   * 
   * @private
   * @type {function}
   */
  #inputCallback = undefined;

  /**
   * A html element to show a "prompt".
   * 
   * @private
   * @type {HTMLElement}
   */
  #$prompt;
  
  /**
   * Constructor
   * Creates a basic terminal simulation on the provided HTMLElement.
   * 
   * @param {HTMLElement} $output - a dom element
   */
  constructor($output) {
    // Store the output DOM element in a local variable.
    this.$output = $output;

    // Clear terminal.
    this.clear();

    // Add the call "terminal" to the $output element.
    this.$output.classList.add('terminal');

    // Create a prompt element.
    // This element gets added if input is needed.
    this.#$prompt = document.createElement("input");
    this.#$prompt.setAttribute("id", "prompt");
    this.#$prompt.setAttribute("type", "text");
    this.#$prompt.setAttribute("length", "50");
    this.#$prompt.addEventListener("keydown", this.#handleKey.bind(this));

    // Force focus on the promt on each click.
    // This is needed for mobile support.
    document.body.addEventListener('click', () => {
      this.#$prompt.focus();
    });
  }

  /**
   * Creates a new HTMLElement with the given text content.
   * This element than gets added to the $output as a new "line".
   * 
   * @private
   * @memberof MinimalTerminal
   * @param {String} text - text that should be displayed in the new "line".
   * @returns {HTMLElement} return a new DOM Element <pre class="line"></pre>
   */
  #newLine(text) {
    const $lineNode = document.createElement("pre");
    $lineNode.classList.add("line");
    $lineNode.innerText = text;
    return $lineNode;
  }

  /**
   * TODO
   * 
   * @private
   * @param {*} e 
   */
  #handleKey(e) {
    // if no input-callback is defined just return
    if (!this.#inputCallback) {
      return;
    }

    if (e.keyCode == 13) {
      const text = this.#$prompt.value;
      this.#$prompt.value = '';
      this.#$prompt.remove();
      this.#inputCallback(text + '\n');
    }
  }

  /**
   * Clear the terminal.
   * Remove all lines.
   * 
   * @public
   */
  clear() {
    this.$output.innerText = "";
  }

  /**
   * Create a new div and add html content.
   * 
   * @public
   * @param {*} htmlContent 
   */
  inserHtml(htmlContent) {
    const $htmlNode = document.createElement("div");
    $htmlNode.innerHTML = htmlContent;
    this.$output.appendChild($htmlNode);
    document.body.scrollTo(0, document.body.scrollHeight);
  }

  /**
   * Write a text to the terminal.
   * By default there is no linebreak at the end of a new line
   * except the line ensd with a "\n".
   * If the given text has multible linebreaks, multibe lines are inserted.
   * 
   * @public
   * @param {string} text 
   */
  write(text) {
    if (!text || text.length <= 0) {
      // empty line
      this.$output.appendChild(document.createElement("br"));
    } else if (text.endsWith("\n")) {
      // single line with linebrank
      const $lineNode = this.#newLine(text);
      this.$output.appendChild(this.#newLine(text));
      this.$output.appendChild(document.createElement("br"));
    } else if (text.includes("\n")) {
      // multible lines
      const lines = text.split("\n");
      lines.forEach((line) => {
        this.write(line);
      });
    } else {
      // single line
      this.$output.appendChild(this.#newLine(text));
    }

    // scroll to the buttom of the page
    document.body.scrollTo(0, document.body.scrollHeight);
  }

  /**
   * Like "write" but with a newline at the end.
   * 
   * @public
   * @param {*} text 
   */
  writeln(text) {
    this.write(text + "\n");
  }

  /**
   * Query from user input.
   * This is done by adding a input-element at the end of the terminal,
   * that showes a prompt and a blinking cursor.
   * If a key is pressed the input is added to the prompt element.
   * The input ends with a linebreak.
   * 
   * @public
   * @param {*} callback 
   */
  input(callback) {
    // show prompt with a blinking prompt
    this.#inputCallback = callback;
    this.$output.appendChild(this.#$prompt);
    this.#$prompt.focus();
  }
}
