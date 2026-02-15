using System;
using System.Windows.Forms;

namespace ControleEmprestimos.Helpers;

/// <summary>
/// Classe auxiliar para configurar controles de formulário
/// </summary>
public static class FormControlHelper
{
    /// <summary>
    /// Converte texto de TextBox para maiúsculas automaticamente
    /// </summary>
    public static void ConfigureTextBoxToUpperCase(TextBox textBox)
    {
        textBox.CharacterCasing = CharacterCasing.Upper;
    }

    /// <summary>
    /// Configura todos os TextBoxes de um formulário para maiúsculas
    /// </summary>
    public static void ConfigureAllTextBoxesToUpperCase(Control container)
    {
        foreach (Control control in container.Controls)
        {
            if (control is TextBox textBox)
            {
                ConfigureTextBoxToUpperCase(textBox);
            }
            else if (control.HasChildren)
            {
                // Recursivamente processar controles filhos
                ConfigureAllTextBoxesToUpperCase(control);
            }
        }
    }

    /// <summary>
    /// Cria e configura um MaskedTextBox para data com navegação automática
    /// </summary>
    public static MaskedTextBox CreateDateMaskedTextBox()
    {
        var mtb = new MaskedTextBox
        {
            Mask = "00/00/0000",
            ValidatingType = typeof(DateTime),
            BeepOnError = false,
            InsertKeyMode = InsertKeyMode.Overwrite
        };

        // Evento para validação e navegação automática
        mtb.KeyPress += (sender, e) =>
        {
            if (sender is MaskedTextBox maskedTextBox)
            {
                // Se pressionar Enter, avançar para próximo controle
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    maskedTextBox.FindForm()?.SelectNextControl(maskedTextBox, true, true, true, true);
                }
            }
        };

        // Validação ao sair do campo
        mtb.TypeValidationCompleted += (sender, e) =>
        {
            if (e.IsValidInput)
            {
                if (e.ReturnValue is DateTime data)
                {
                    // Validar data razoável
                    if (data.Year < 1900 || data.Year > 2100)
                    {
                        e.Cancel = true;
                        MessageBox.Show(
                            "Por favor, informe uma data válida entre 1900 e 2100.",
                            "Data Inválida",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(((MaskedTextBox)sender!).Text.Replace("/", "").Replace(" ", "")))
            {
                // Só mostrar erro se realmente digitou algo
                MessageBox.Show(
                    "Data inválida. Use o formato: DD/MM/AAAA",
                    "Data Inválida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        };

        // Ao ganhar foco, posicionar no início
        mtb.Enter += (sender, e) =>
        {
            if (sender is MaskedTextBox maskedTextBox)
            {
                maskedTextBox.SelectionStart = 0;
            }
        };

        return mtb;
    }

    /// <summary>
    /// Substitui DateTimePicker por MaskedTextBox para data
    /// </summary>
    public static MaskedTextBox ReplaceDateTimePickerWithMaskedTextBox(
        Control container,
        string dateTimePickerName,
        Point location,
        Size size,
        int tabIndex)
    {
        // Encontrar o DateTimePicker
        var dtp = container.Controls.Find(dateTimePickerName, false).FirstOrDefault() as DateTimePicker;
        
        if (dtp != null)
        {
            // Criar MaskedTextBox
            var mtb = CreateDateMaskedTextBox();
            mtb.Name = dateTimePickerName.Replace("dtp", "mtb");
            mtb.Location = location;
            mtb.Size = size;
            mtb.TabIndex = tabIndex;
            mtb.Font = dtp.Font;
            
            // Copiar valor se houver
            mtb.Text = dtp.Value.ToString("dd/MM/yyyy");
            
            // Remover DateTimePicker e adicionar MaskedTextBox
            container.Controls.Remove(dtp);
            container.Controls.Add(mtb);
            dtp.Dispose();
            
            return mtb;
        }
        else
        {
            // Se não encontrou o DTP, criar novo MaskedTextBox
            var mtb = CreateDateMaskedTextBox();
            mtb.Name = dateTimePickerName.Replace("dtp", "mtb");
            mtb.Location = location;
            mtb.Size = size;
            mtb.TabIndex = tabIndex;
            container.Controls.Add(mtb);
            
            return mtb;
        }
    }

    /// <summary>
    /// Substitui todos os DateTimePickers de um controle por MaskedTextBox
    /// </summary>
    public static void ReplaceAllDateTimePickersWithMaskedTextBox(Control container)
    {
        var dateTimePickers = new List<DateTimePicker>();
        
        // Encontrar todos os DateTimePickers
        FindAllDateTimePickers(container, dateTimePickers);
        
        // Substituir cada um
        foreach (var dtp in dateTimePickers)
        {
            ReplaceDateTimePickerWithMaskedTextBox(dtp);
        }
    }

    private static void FindAllDateTimePickers(Control container, List<DateTimePicker> found)
    {
        foreach (Control control in container.Controls)
        {
            if (control is DateTimePicker dtp)
            {
                found.Add(dtp);
            }
            else if (control.HasChildren)
            {
                FindAllDateTimePickers(control, found);
            }
        }
    }

    private static void ReplaceDateTimePickerWithMaskedTextBox(DateTimePicker dtp)
    {
        var parent = dtp.Parent;
        if (parent == null) return;

        // Salvar propriedades
        var location = dtp.Location;
        var size = dtp.Size;
        var tabIndex = dtp.TabIndex;
        var name = dtp.Name;
        var value = dtp.Value;
        var enabled = dtp.Enabled;

        // Criar MaskedTextBox
        var mtb = CreateDateMaskedTextBox();
        mtb.Name = name.Replace("dtp", "mtb");
        mtb.Location = location;
        mtb.Size = size;
        mtb.TabIndex = tabIndex;
        mtb.Text = value.ToString("dd/MM/yyyy");
        mtb.Enabled = enabled;

        // Remover DateTimePicker e adicionar MaskedTextBox
        parent.Controls.Remove(dtp);
        parent.Controls.Add(mtb);
        dtp.Dispose();
    }

    /// <summary>
    /// Obtém DateTime de um MaskedTextBox
    /// </summary>
    public static DateTime? GetDateFromMaskedTextBox(MaskedTextBox mtb)
    {
        if (mtb.MaskCompleted && DateTime.TryParse(mtb.Text, out DateTime result))
        {
            return result;
        }
        return null;
    }

    /// <summary>
    /// Define DateTime em um MaskedTextBox
    /// </summary>
    public static void SetDateToMaskedTextBox(MaskedTextBox mtb, DateTime date)
    {
        mtb.Text = date.ToString("dd/MM/yyyy");
    }

    /// <summary>
    /// Configura DateTimePicker para navegação com Enter (fallback)
    /// </summary>
    public static void ConfigureDateTimePickerNavigation(DateTimePicker dtp)
    {
        dtp.Format = DateTimePickerFormat.Short;
        
        // Adicionar evento KeyDown para navegação com Enter
        dtp.KeyDown += (sender, e) =>
        {
            if (sender is DateTimePicker picker)
            {
                // Se pressionar Enter, avançar para próximo controle
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    picker.FindForm()?.SelectNextControl(picker, true, true, true, true);
                }
            }
        };

        // Adicionar evento ValueChanged para validação suave
        dtp.ValueChanged += (sender, e) =>
        {
            if (sender is DateTimePicker picker)
            {
                // Garantir que a data esteja no formato correto
                if (picker.Value.Date < new DateTime(1900, 1, 1))
                {
                    picker.Value = DateTime.Now.Date;
                }
            }
        };
    }

    /// <summary>
    /// Configura todos os DateTimePickers de um formulário
    /// </summary>
    public static void ConfigureAllDateTimePickers(Control container)
    {
        foreach (Control control in container.Controls)
        {
            if (control is DateTimePicker dtp)
            {
                ConfigureDateTimePickerNavigation(dtp);
            }
            else if (control.HasChildren)
            {
                // Recursivamente processar controles filhos
                ConfigureAllDateTimePickers(control);
            }
        }
    }

    /// <summary>
    /// Encontra MaskedTextBox de data pelo nome original do DateTimePicker
    /// </summary>
    public static MaskedTextBox? FindDateMaskedTextBox(Control container, string dateTimePickerName)
    {
        var maskedTextBoxName = dateTimePickerName.Replace("dtp", "mtb");
        var controls = container.Controls.Find(maskedTextBoxName, true);
        return controls.FirstOrDefault() as MaskedTextBox;
    }

    /// <summary>
    /// Configura ComboBox para texto em maiúsculas
    /// </summary>
    public static void ConfigureComboBoxToUpperCase(ComboBox comboBox)
    {
        // Para ComboBoxes editáveis
        if (comboBox.DropDownStyle == ComboBoxStyle.DropDown)
        {
            comboBox.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar))
                {
                    e.KeyChar = char.ToUpper(e.KeyChar);
                }
            };
        }
    }

    /// <summary>
    /// Configura todos os controles de um formulário de uma vez
    /// </summary>
    public static void ConfigureForm(Form form)
    {
        form.Load += (sender, e) =>
        {
            ConfigureAllTextBoxesToUpperCase(form);
            ConfigureAllDateTimePickers(form);
        };
    }
}
