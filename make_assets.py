# -*- coding: utf-8 -*-
"""Генерує pizza.png (для PictureBox) та скріншот форми pizza_form.png (для звіту)."""
from PIL import Image, ImageDraw, ImageFont

DIR = "/Users/mac/PracticeWork2/"
TAHOMA = "/System/Library/Fonts/Supplemental/Tahoma.ttf"


def tah(sz):
    return ImageFont.truetype(TAHOMA, sz)


# ---------------- 1. Зображення піци ----------------
S = 240
img = Image.new("RGBA", (S, S), (255, 255, 255, 0))
d = ImageDraw.Draw(img)
cx = cy = S // 2
d.ellipse([14, 14, S - 14, S - 14], fill="#e6a85a", outline="#c98b3a", width=3)   # корж
d.ellipse([30, 30, S - 30, S - 30], fill="#cf4030")                               # соус
d.ellipse([40, 40, S - 40, S - 40], fill="#f4c64e")                               # сир
# пепероні
pepper = [(80, 75), (160, 80), (120, 130), (75, 160), (165, 160), (120, 60)]
for px, py in pepper:
    d.ellipse([px - 14, py - 14, px + 14, py + 14], fill="#9e241c", outline="#7d1a13", width=2)
# зелень
for gx, gy in [(110, 95), (140, 150), (95, 130)]:
    d.ellipse([gx - 4, gy - 4, gx + 4, gy + 4], fill="#3a7d2c")
img.save(DIR + "pizza.png")
print("pizza.png", img.size)


# ---------------- 2. Скріншот форми (для звіту) ----------------
W, H = 556, 470
TB = 30
im = Image.new("RGB", (W, H), "#f0f0f0")
g = ImageDraw.Draw(im)
g.rectangle([0, 0, W - 1, H - 1], outline="#7a7a7a")
g.rectangle([1, 1, W - 2, TB], fill="#ffffff", outline="#d0d0d0")
g.text((10, 7), "Замовлення піци та напоїв", font=tah(13), fill="#202020")
for i, sym in enumerate(["—", "☐", "✕"]):
    g.text((W - 96 + i * 32, 6), sym, font=tah(14), fill="#404040")

# TabControl
tx, ty, tw, th = 12, TB + 8, 516, 300
g.rectangle([tx, ty, tx + tw, ty + th], fill="#ffffff", outline="#999999")
# вкладки
g.rectangle([tx, ty, tx + 70, ty + 24], fill="#ffffff", outline="#999999")
g.text((tx + 18, ty + 4), "Піца", font=tah(12), fill="#000")
g.rectangle([tx + 70, ty + 2, tx + 140, ty + 24], fill="#e8e8e8", outline="#999999")
g.text((tx + 84, ty + 4), "Напої", font=tah(12), fill="#000")

def field(x, y, w, h, text="", fill="#ffffff"):
    g.rectangle([x, y, x + w, y + h], fill=fill, outline="#7a9bbf")
    if text:
        g.text((x + 5, y + 3), text, font=tah(12), fill="#000")

def updown(x, y, w, text):
    g.rectangle([x, y, x + w, y + 22], fill="#ffffff", outline="#7a9bbf")
    g.text((x + 5, y + 3), text, font=tah(12), fill="#000")
    g.rectangle([x + w - 15, y, x + w, y + 22], fill="#e1e1e1", outline="#7a9bbf")
    g.text((x + w - 12, y - 1), "▲", font=tah(8), fill="#404040")
    g.text((x + w - 12, y + 9), "▼", font=tah(8), fill="#404040")

# заголовки колонок
cy0 = ty + 30
g.text((tx + 15, cy0), "Назва", font=tah(12), fill="#404040")
g.text((tx + 150, cy0), "Кількість", font=tah(12), fill="#404040")
g.text((tx + 240, cy0), "Ціна, грн", font=tah(12), fill="#404040")
rows = [("Маргарита", "2", "120"), ("Пепероні", "1", "150"), ("Гавайська", "0", "140")]
ry = cy0 + 28
for name, qty, price in rows:
    g.text((tx + 15, ry + 3), name, font=tah(12), fill="#000")
    updown(tx + 150, ry, 70, qty)
    field(tx + 240, ry, 70, 22, price)
    ry += 34

# PictureBox з піцою
pic = Image.open(DIR + "pizza.png").convert("RGBA").resize((150, 150))
im.paste(pic, (tx + 320, ty + 20), pic)
g.rectangle([tx + 320, ty + 20, tx + 470, ty + 170], outline="#7a7a7a")

# Підсумок
g.text((12, TB + 322), "Вартість замовлення: 390,00 грн", font=tah(14), fill="#202020")

# FlowLayoutPanel з кнопками
fy = TB + 352
g.rectangle([12, fy, 528, fy + 50], outline="#999999")
def button(x, y, w, text):
    g.rectangle([x, y, x + w, y + 34], fill="#e1e1e1", outline="#adadad")
    tb = g.textbbox((0, 0), text, font=tah(12)); tw2 = tb[2] - tb[0]
    g.text((x + (w - tw2) // 2, y + 9), text, font=tah(12), fill="#202020")
button(22, fy + 8, 150, "Замовити")
button(184, fy + 8, 150, "Відмінити")
button(346, fy + 8, 150, "Вихід")

im.save(DIR + "pizza_form.png")
print("pizza_form.png", im.size)
