// ======= FII CUSTOM FORM INPUT =======
// === FII CUSTOM SELECT ===
function FII_InitCustomSelect() {
    var fii_form_selects = document.getElementsByClassName('fii-form-select'),
        form_select_count = fii_form_selects.length,
        current_select,
        current_select_length,
        fii_option_selected,
        fii_option_items,
        fii_option_content;

    for (let iSelect = 0; iSelect < form_select_count; iSelect++) {
        current_select = fii_form_selects[iSelect].getElementsByTagName('select')[0];
        current_select_length = current_select.length;

        fii_option_selected = document.createElement('DIV');
        fii_option_selected.setAttribute('class', 'fii-option-selected justify-content-center align-items-center d-flex flex-column');
        fii_option_selected.innerHTML = current_select.options[current_select.selectedIndex].innerHTML;
        fii_form_selects[iSelect].appendChild(fii_option_selected);

        fii_option_items = document.createElement('DIV');
        fii_option_items.setAttribute('class', 'fii-option-items fii-option-close');

        for (let iOption = 0; iOption < current_select_length; iOption++) {
            fii_option_content = document.createElement('DIV');
            fii_option_content.innerHTML = current_select.options[iOption].innerHTML;
            fii_option_content.addEventListener('click', function (e) {
                let current_select_node = this.parentNode.parentNode.getElementsByTagName('select')[0],
                    current_select_node_count = current_select_node.length;
                //console.log(current_select_node);
                let current_option_item = this.parentNode.previousSibling;

                for (let iOptContent = 0; iOptContent < current_select_node_count; iOptContent++) {
                    //console.log(this.innerHTML);
                    if (current_select_node.options[iOptContent].innerHTML == this.innerHTML) {
                        current_select_node.selectedIndex = iOptContent;
                        current_option_item.innerHTML = this.innerHTML;

                        let list_previous_selected = this.parentNode.getElementsByClassName('fii-option-mark-selected'),
                            count_previous_selected = list_previous_selected.length;
                        for (let iPreviousSelected = 0; iPreviousSelected < count_previous_selected; iPreviousSelected++) {
                            list_previous_selected[iPreviousSelected].removeAttribute('class');
                        }

                        this.setAttribute('class', 'fii-option-mark-selected');
                        break;
                    }
                }
                current_option_item.click();
            });


            fii_option_items.appendChild(fii_option_content);
        }

        fii_form_selects[iSelect].appendChild(fii_option_items);
        fii_option_selected.addEventListener('click', function (e) {
            e.stopPropagation();
            closeAllSelectField(this);
            this.nextSibling.classList.toggle('fii-option-close');
            this.classList.toggle('fii-option-open');
        });
    }

    function closeAllSelectField(inputElem) {
        let func_option_selected = document.getElementsByClassName('fii-option-selected'),
            func_option_items = document.getElementsByClassName('fii-option-items');
        let option_selected_count = func_option_selected.length,
            option_items_count = func_option_items.length;
        let idxStorage = [];
        for (let iItem = 0; iItem < option_items_count; iItem++) {
            if (inputElem == func_option_selected[iItem]) {
                idxStorage.push(iItem);
            }
            else {
                func_option_selected[iItem].classList.remove('fii-option-open');
            }
        }
        for (let idx = 0; idx < option_selected_count; idx++) {
            if (idxStorage.indexOf(idx)) {
                func_option_items[idx].classList.add('fii-option-close');
            }
        }
    };

    document.addEventListener('click', closeAllSelectField);
};

// === ===