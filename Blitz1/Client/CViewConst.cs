namespace Blitz1
{
    internal static class CViewConst
    {
        public const double SEPARATOR_COLUMN_WIDTH = 3.14;
        public const double SEPARATOR_ROW_HEIGHT = 27.0;
        public const double PARTICIPANTS_COLUMN_WIDTH = 25.43;
        public const double PARTICIPANT_ROW_HEIGHT = 13.50;
        public const double PARTICIPANT_CAPTION_ROW_HEIGHT = 93.75;
        public const double SCORE_COLUMN_WIDTH = 2.25;
        public const double WIDE_SCORE_COLUMN_WIDTH = 4.25;
        public const double COMMENT_COLUMN_WIDTH = 10;

        public const int EXTRA_COLUMNS = 10; //забито, пропущено, разница, В, Н, П, очки, игры, место, комментарий
        public const int COLUMN_PLUS_OFFSET = -EXTRA_COLUMNS + 1; //смещение колонки забитых от конца таблицы
        public const int COLUMN_MINUS_OFFSET = COLUMN_PLUS_OFFSET + 1; //смещение колонки пропущенных от конца таблицы
        public const int COLUMN_DIFF_OFFSET = COLUMN_MINUS_OFFSET + 1; //смещение колонки разницы от конца таблицы
        public const int COLUMN_WIN_OFFSET = COLUMN_DIFF_OFFSET + 1; //смещение колонки В от конца таблицы
        public const int COLUMN_DRAW_OFFSET = COLUMN_WIN_OFFSET + 1; //смещение колонки Н от конца таблицы
        public const int COLUMN_LOST_OFFSET = COLUMN_DRAW_OFFSET + 1; //смещение колонки П от конца таблицы
        public const int COLUMN_POINTS_OFFSET = COLUMN_LOST_OFFSET + 1; //смещение колонки очков от конца таблицы
        public const int COLUMN_MATCHES_OFFSET = COLUMN_POINTS_OFFSET + 1; //смещение колонки количества игр от конца таблицы
        public const int COLUMN_PLACES_OFFSET = COLUMN_MATCHES_OFFSET + 1; //смещение колонки места от конца таблицы
        public const int COLUMN_COMMENT_OFFSET = COLUMN_PLACES_OFFSET + 1; //смещение колонки комментария от конца таблицы

    }
}
