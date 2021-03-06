PGDMP                          z            mctg    14.1    14.1     ?           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            ?           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            ?           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            ?           1262    16384    mctg    DATABASE     O   CREATE DATABASE mctg WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'C';
    DROP DATABASE mctg;
                postgres    false            ?            1259    16385    Player    TABLE     +  CREATE TABLE public."Player" (
    "Username" character(50),
    "Password" character(50),
    "Coins" integer,
    "ELO" integer,
    "Loss" integer,
    "Wins" integer,
    "Draws" integer,
    "Name" character(50),
    "ID" integer NOT NULL,
    "Bio" character(50),
    "Image" character(50)
);
    DROP TABLE public."Player";
       public         heap    postgres    false            ?            1259    16394    PlayerCards    TABLE       CREATE TABLE public."PlayerCards" (
    "ID" integer NOT NULL,
    "Name" character(50),
    "Dmg" double precision,
    "Element" integer,
    "Type" integer,
    "Package_ID" integer,
    "isBattleDeck" boolean,
    "Player_ID" character(50),
    "ID_name" character(50)
);
 !   DROP TABLE public."PlayerCards";
       public         heap    postgres    false            ?            1259    32815    PlayerCards_ID_seq    SEQUENCE     ?   ALTER TABLE public."PlayerCards" ALTER COLUMN "ID" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."PlayerCards_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    210            ?            1259    32785    Player_ID_seq    SEQUENCE     ?   ALTER TABLE public."Player" ALTER COLUMN "ID" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Player_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    209            ?          0    16385    Player 
   TABLE DATA           ?   COPY public."Player" ("Username", "Password", "Coins", "ELO", "Loss", "Wins", "Draws", "Name", "ID", "Bio", "Image") FROM stdin;
    public          postgres    false    209   ?       ?          0    16394    PlayerCards 
   TABLE DATA           ?   COPY public."PlayerCards" ("ID", "Name", "Dmg", "Element", "Type", "Package_ID", "isBattleDeck", "Player_ID", "ID_name") FROM stdin;
    public          postgres    false    210   ?       ?           0    0    PlayerCards_ID_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public."PlayerCards_ID_seq"', 714, true);
          public          postgres    false    212            ?           0    0    Player_ID_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Player_ID_seq"', 164, true);
          public          postgres    false    211            d           2606    32790    PlayerCards PlayerCards_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public."PlayerCards"
    ADD CONSTRAINT "PlayerCards_pkey" PRIMARY KEY ("ID");
 J   ALTER TABLE ONLY public."PlayerCards" DROP CONSTRAINT "PlayerCards_pkey";
       public            postgres    false    210            b           2606    32787    Player Player_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."Player"
    ADD CONSTRAINT "Player_pkey" PRIMARY KEY ("ID");
 @   ALTER TABLE ONLY public."Player" DROP CONSTRAINT "Player_pkey";
       public            postgres    false    209            ?   9   x???L?K?OMV p?$?e?????Ȁ????I?	?ohfB?6ҵp??qqq ??&X      ?      x?????? ? ?     