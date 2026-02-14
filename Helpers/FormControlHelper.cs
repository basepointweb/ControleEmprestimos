using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ControleEmprestimos.Helpers;

/// <summary>
/// Classe auxiliar para configurar controles de formulário
/// </summary>
public static class FormControlHelper
{
    // Import necessário para simular teclas
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    private const int VK_RIGHT = 0x27;
    private const uint KEYEVENTF_KEYUP = 0x0002;

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
    /// Configura DateTimePicker para navegação automática ao digitar
    /// </summary>
    public static void ConfigureDateTimePickerNavigation(DateTimePicker dtp)
    {
        dtp.Format = DateTimePickerFormat.Short;
        
        // Variável para rastrear digitação
        string lastText = string.Empty;
        int digitCount = 0;
        string currentPart = "day"; // day, month, year
        
        // Evento KeyPress para detectar digitação
        dtp.KeyPress += (sender, e) =>
        {
            if (sender is DateTimePicker picker)
            {
                // Apenas processar dígitos
                if (char.IsDigit(e.KeyChar))
                {
                    digitCount++;
                    
                    // Criar um timer para processar após a digitação ser aplicada
                    var timer = new System.Threading.Timer(_ =>
                    {
                        picker.Invoke((Action)(() =>
                        {
                            try
                            {
                                // Verificar qual parte está sendo editada
                                var currentText = picker.Text;
                                
                                // Se digitou 2 dígitos, avançar automaticamente
                                if (digitCount == 2)
                                {
                                    // Simular seta para direita para avançar
                                    SendRightArrow();
                                    digitCount = 0;
                                }
                            }
                            catch { }
                        }));
                    }, null, 50, System.Threading.Timeout.Infinite);
                }
                else
                {
                    digitCount = 0;
                }
            }
        };

        // Evento KeyDown para outras teclas
        dtp.KeyDown += (sender, e) =>
        {
            if (sender is DateTimePicker picker)
            {
                // Se pressionar Enter, avançar para próximo controle
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    picker.FindForm()?.SelectNextControl(picker, true, true, true, true);
                    digitCount = 0;
                }
                // Resetar contador em outras teclas de navegação
                else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || 
                         e.KeyCode == Keys.Tab || e.KeyCode == Keys.Back)
                {
                    digitCount = 0;
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

        // Resetar contador quando ganhar foco
        dtp.Enter += (sender, e) =>
        {
            digitCount = 0;
            currentPart = "day";
        };

        // Resetar contador quando perder foco
        dtp.Leave += (sender, e) =>
        {
            digitCount = 0;
        };
    }

    /// <summary>
    /// Simula o pressionamento da seta direita
    /// </summary>
    private static void SendRightArrow()
    {
        // Pressionar seta direita
        keybd_event(VK_RIGHT, 0, 0, UIntPtr.Zero);
        // Soltar seta direita
        keybd_event(VK_RIGHT, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
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
