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
  #$prompt = undefined;
  
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
    // This element gets added if input is needed
    this.#$prompt = document.createElement("span");
    this.#$prompt.setAttribute("id", "prompt");
    this.#$prompt.innerText = "";

    //TODO: this handler shouls be only on the propt element and only active if cursor is visible
    document.addEventListener("keyup", this.#handleKey.bind(this));
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
    // if no input-callback is defined 
    if (!this.#inputCallback) {
      return;
    }

    if (e.keyCode === 13 /* ENTER */) {
      // create a new line with the text input and remove the prompt
      const text = this.#$prompt.innerText;
      this.write(text + "\n");
      this.#$prompt.innerText = "";
      this.#$prompt.remove();

      // return the inputed text
      this.#inputCallback(text);
      
      // remove the callback and the key handler
      this.#inputCallback = undefined;
    } else if (e.keyCode === 8 /* BACKSPACE */) {
      this.#$prompt.innerText = this.#$prompt.innerText.slice(0, -1);
    } else {
      this.#$prompt.innerHtml = '';
      this.#$prompt.innerText =  this.#$prompt.innerText + e.key;
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
   * TODO:
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
    if (text.match(/^\n*$/)) {
      // empty new line
      text.match(/\n/g).forEach(() => {
        const $br = document.createElement("br");
        this.$output.appendChild($br);
      });
    } else if (text && text.length && text.includes("\n")) {
      const lines = text.split("\n");
      lines.forEach((line) => {
        if (line.length === 0 || line.match(/^\s*$/)) {
          this.$output.appendChild(document.createElement("br"));
        } else {
          const $lineNode = this.#newLine(line);
          this.$output.appendChild($lineNode);
          //this.$node.appendChild(document.createElement("br"));
        }
      });
    } else if (text && text.length) {
      // simple line
      const $lineNode = this.#newLine(text);
      this.$output.appendChild($lineNode);
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
    this.$output.appendChild(this.#$prompt);
    this.#inputCallback = callback;
  }
}
