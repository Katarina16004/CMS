# 📚 Content Management System (CMS) – WPF Project

## 📌 Project Description
It provides a local solution for managing digital content on a chosen theme. The system supports two user roles:
- **Admin** – can add, edit, and delete content.
- **Visitor** – can only browse and view content.

All content is stored locally using **XML** for data persistence and **RTF** files for text documents. The application ensures a clear separation of roles, user-friendly interaction, and a visually consistent UI.

---

## ✨ Features
- **User login system**  
  - Predefined usernames and passwords stored in XML.  
  - `Admin` and `Visitor` roles with different access rights.  

- **Content management**  
  - Content items include:  
    - Numeric field (entered by user)  
    - Text field  
    - Image path (with live preview)  
    - Rich text reference (`.rtf` file)  
    - Date of creation (auto-generated)  
  - All content is stored in one XML file.  

- **Table view**  
  - Checkbox column for deleting multiple items.  
  - Hyperlink column for detailed view / editing.  
  - Image column with thumbnail.  
  - Date column.  
  - Master checkbox for "select all".  

- **Content editor**  
  - Add / Edit windows with:  
    - Image preview.  
    - RichTextBox editor supporting:  
      - **Bold, Italic, Underline**  
      - Font selection (with preview)  
      - Font size selection  
      - Text color (with live preview of system colors)  
      - Word counter (status bar)  
  - Automatic saving of `.rtf` files with entered content.  

- **User experience**  
  - Consistent color palette, icons, and UI theme.  
  - Tooltips, cursor changes, and toast/MessageBox feedback for actions.  
  - Confirmation before deletion, validation messages below fields.
    
---

## 🎨 Design Guidelines
- Consistent theme (colors, fonts, shapes).  
- Responsive and clear layout.  
- Good readability (contrast between text and background). 
